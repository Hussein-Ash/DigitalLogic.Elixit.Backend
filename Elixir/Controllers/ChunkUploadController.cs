using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Elixir.Services.StaticService;
using Elixir.Services.UploadAndVideoServise;

namespace Elixir.Controllers
{
    [ApiController]
    [Route("api/chunks")]
    public class ChunkUploadController(IChunkUploadService chunkUploadService) : ControllerBase
    {

        [HttpPost("upload")]
        public async Task<IActionResult> UploadChunk([FromForm] string uploadId, [FromForm] int chunkIndex, [FromForm] IFormFile chunk)
        {
            if (chunk == null || chunk.Length == 0)
                return BadRequest("Chunk cannot be empty.");

            using (var stream = chunk.OpenReadStream())
            {
                await chunkUploadService.SaveChunkAsync(uploadId, chunkIndex, stream);
            }

            return Ok(new { message = "Chunk uploaded successfully." });
        }

        [HttpPost("assemble")]
        public async Task<IActionResult> AssembleFile([FromForm] string uploadId, [FromForm] string fileName)
        {
            try
            {
                var assembledFilePath = await chunkUploadService.AssembleChunksAsync(uploadId, fileName);

                var processedFilePath = await chunkUploadService.ProcessVideoAsync(assembledFilePath);

                var s3Key = $"processed/{fileName}/output.m3u8";
                var s3Url = await chunkUploadService.UploadToS3Async(processedFilePath, s3Key);

                return Ok(new { message = "File uploaded successfully.", path = s3Url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error assembling file.", error = ex.Message });
            }
        }
    }
}
