using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Ecommerce.Api.Blobs;
using Ecommerce.Api.Interfaces;

namespace Ecommerce.Api.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _svc;
        private readonly BlobContainerClient _container;

        public BlobStorageService(BlobServiceClient svc, BlobContainerOptions opts)
        {
            _svc = svc;
            _container = _svc.GetBlobContainerClient(opts.ContainerName);
            _container.CreateIfNotExists(PublicAccessType.None); // contenedor privado
        }

        public async Task<string> UploadAsync(Stream content, string fileName, string contentType, string extension, CancellationToken ct = default)
        {
            // Opcional: normalizar nombre
            var safeName = $"{Guid.NewGuid():N}{extension}".ToLowerInvariant();

            var blob = _container.GetBlobClient(safeName);
            var headers = new BlobHttpHeaders { ContentType = contentType };

            await blob.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers }, ct);
            return safeName; // Guarda este nombre en tu BD si lo necesitas
        }

        public async Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
        {
            var blob = _container.GetBlobClient(blobName);
            Response<BlobDownloadResult> res = await blob.DownloadContentAsync(ct);
            return new MemoryStream(res.Value.Content.ToArray()); // para devolver como FileStreamResult
        }

        public async Task DeleteAsync(string blobName, CancellationToken ct = default)
        {
            var blob = _container.GetBlobClient(blobName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, conditions: null, ct);
        }

        public async Task<IReadOnlyList<string>> ListAsync(string? prefix = null, CancellationToken ct = default)
        {
            var list = new List<string>();
            await foreach (var item in _container.GetBlobsAsync(prefix: prefix, cancellationToken: ct))
                list.Add(item.Name);
            return list;
        }
    }
}
