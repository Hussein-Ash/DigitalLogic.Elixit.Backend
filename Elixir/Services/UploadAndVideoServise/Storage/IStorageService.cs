namespace Elixir.Services.StaticService.Storage;

public interface IStorageService
{
    Task UploadFileAsync(string bucketName, string key, string filePath, string contentType);
    string GetFileUrl(string bucketName, string key);
}


