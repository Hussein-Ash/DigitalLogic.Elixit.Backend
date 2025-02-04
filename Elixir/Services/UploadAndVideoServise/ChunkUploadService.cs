using Elixir.Services.StaticService.Storage;
using FFMpegCore;

namespace Elixir.Services.UploadAndVideoServise
{
    public interface IChunkUploadService
    {
        Task SaveChunkAsync(string uploadId, int chunkIndex, Stream chunkStream);
        Task<string> AssembleChunksAsync(string uploadId, string fileName);
        Task<List<string>> ProcessVideoAsync(string filePath);
        Task<Dictionary<string, string>> UploadToS3Async(List<string> localFilePaths, string fileName);
    }


    public class ChunkUploadService : IChunkUploadService
    {
        private readonly string _uploadRootDirectory;
        private readonly string _bucketName = "your-bucket-name";
        private readonly IStorageService _storageService;

        public ChunkUploadService(IStorageService storageService)
        {
            _uploadRootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            _storageService = storageService;

            if (!Directory.Exists(_uploadRootDirectory))
                Directory.CreateDirectory(_uploadRootDirectory);
        }

        public async Task SaveChunkAsync(string uploadId, int chunkIndex, Stream chunkStream)
        {
            var uploadFolder = Path.Combine(_uploadRootDirectory, uploadId);
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var chunkPath = Path.Combine(uploadFolder, $"{chunkIndex}.part");

            using (var fileStream = new FileStream(chunkPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await chunkStream.CopyToAsync(fileStream);
            }
        }

        public async Task<string> AssembleChunksAsync(string uploadId, string fileName)
        {
            var uploadFolder = Path.Combine(_uploadRootDirectory, uploadId);
            if (!Directory.Exists(uploadFolder))
                throw new DirectoryNotFoundException("Upload folder not found.");

            var assembledFilePath = Path.Combine(_uploadRootDirectory, fileName);

            using (var outputStream =
                   new FileStream(assembledFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var chunkFiles = Directory.GetFiles(uploadFolder)
                    .OrderBy(file => int.Parse(Path.GetFileNameWithoutExtension(file)));

                foreach (var chunkFile in chunkFiles)
                {
                    using (var chunkStream = new FileStream(chunkFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await chunkStream.CopyToAsync(outputStream);
                    }
                }
            }

            // Cleanup chunk files
            Directory.Delete(uploadFolder, true);

            return assembledFilePath;
        }

        public async Task<List<string>> ProcessVideoAsync(string filePath)
        {
            // var videoId = Path.GetFileNameWithoutExtension(filePath); // Extract a unique identifier
            var videoFolder = Path.Combine(_uploadRootDirectory, "processed", Guid.NewGuid().ToString());

            if (!Directory.Exists(videoFolder))
                Directory.CreateDirectory(videoFolder);

            var resolutions = new (string name, string scale, string bitrate)[]
            {
                ("1080p", "1080:1920", "5000k"),
                ("720p", "720:1280", "2500k"),
                ("480p", "480:864", "1000k")
            };

            var tasks = resolutions.Select(async resolution =>
            {
                var (name, scale, bitrate) = resolution;
                var resolutionFolder = Path.Combine(videoFolder, name);
                if (!Directory.Exists(resolutionFolder))
                    Directory.CreateDirectory(resolutionFolder);

                var outputPlaylist = Path.Combine(resolutionFolder, "output.m3u8");

                await FFMpegArguments
                    .FromFileInput(filePath)
                    .OutputToFile(outputPlaylist, true, options => options
                        .WithVideoCodec("libx264")
                        .WithAudioCodec("aac")
                        .WithArgument(new FFMpegCore.Arguments.CustomArgument($"-vf \"scale={scale}\""))
                        .WithArgument(new FFMpegCore.Arguments.CustomArgument($"-preset ultrafast"))
                        .WithArgument(new FFMpegCore.Arguments.CustomArgument($"-movflags +faststart"))
                        .WithArgument(new FFMpegCore.Arguments.CustomArgument($"-f hls"))
                        .WithArgument(new FFMpegCore.Arguments.CustomArgument($"-hls_time 4"))
                        .WithArgument(new FFMpegCore.Arguments.CustomArgument($"-hls_playlist_type vod"))
                        .WithArgument(
                            new FFMpegCore.Arguments.CustomArgument(
                                $"-hls_segment_filename \"{resolutionFolder}/segment_%03d.ts\"")))
                    .ProcessAsynchronously();

                return outputPlaylist;
            });
            
            
            var thumbnailPath = await ExtractThumbnailAsync(filePath);
            
            var urls = await UploadToS3Async(tasks.Select(t => t.Result).ToList(), Path.GetFileName(filePath)); 
            urls["thumbnail"] = thumbnailPath;

            var processedFiles = await Task.WhenAll(tasks);
            return processedFiles.ToList();
        }

        public async Task<string> ExtractThumbnailAsync(string filePath, int seconds = 1)
        {
            var thumbnailFolder = Path.Combine(_uploadRootDirectory, "thumbnails");

            if (!Directory.Exists(thumbnailFolder))
                Directory.CreateDirectory(thumbnailFolder);

            var thumbnailPath = Path.Combine(thumbnailFolder, $"{Path.GetFileNameWithoutExtension(filePath)}.jpg");

            await FFMpegArguments
                .FromFileInput(filePath)
                .OutputToFile(thumbnailPath, overwrite: true, options => options
                    .WithVideoCodec("mjpeg")
                    .WithFrameOutputCount(1)
                    .WithCustomArgument($"-vf \"thumbnail,scale=320:180\" -ss {seconds} -frames:v 1"))
                .ProcessAsynchronously();

            return thumbnailPath;
        }

        
        public async Task<Dictionary<string, string>> UploadToS3Async(List<string> localFilePaths, string fileName)
        {
            var videoId = Path.GetFileNameWithoutExtension(fileName);
            var urls = new Dictionary<string, string>();

            var tasks = localFilePaths.Select(async filePath =>
            {
                string resolution = new DirectoryInfo(Path.GetDirectoryName(filePath)!).Name;
                string key = $"processed/{fileName}/{resolution}/output.m3u8";

                await _storageService.UploadFileAsync(_bucketName, key, filePath, 
                    filePath.EndsWith(".m3u8") ? "application/vnd.apple.mpegurl" : "video/mp2t");

                urls[resolution] = _storageService.GetFileUrl(_bucketName, key);
            });

            await Task.WhenAll(tasks);
            return urls;
        }
    }
}