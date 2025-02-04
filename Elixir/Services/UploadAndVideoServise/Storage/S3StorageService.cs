using Amazon.S3;
using Amazon.S3.Transfer;

namespace Elixir.Services.StaticService.Storage;

public class S3StorageService(IAmazonS3 s3Client) : IStorageService
{
    public async Task UploadFileAsync(string bucketName, string key, string filePath, string contentType)
    {
        using var fileTransferUtility = new TransferUtility(s3Client);

        await fileTransferUtility.UploadAsync(new TransferUtilityUploadRequest
        {
            BucketName = bucketName,
            FilePath = filePath,
            Key = key,
            ContentType = contentType
        });
    }

    public string GetFileUrl(string bucketName, string key)
    {
        return $"https://{bucketName}.s3.amazonaws.com/{key}";
    }
}
