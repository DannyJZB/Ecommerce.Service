namespace Ecommerce.Api.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(Stream content, string fileName, string contentType, string extension, CancellationToken ct = default);
        Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default);
        Task DeleteAsync(string blobName, CancellationToken ct = default);
        Task<IReadOnlyList<string>> ListAsync(string? prefix = null, CancellationToken ct = default);
    }
}
