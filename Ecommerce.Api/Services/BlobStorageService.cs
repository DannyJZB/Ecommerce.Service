using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Ecommerce.Api.Blobs;
using Ecommerce.Api.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Api.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _svc;
        private readonly BlobContainerClient _container;
        private readonly IConfiguration _config;

        public BlobStorageService(BlobServiceClient svc, BlobContainerOptions opts, IConfiguration config)
        {
            _svc = svc;
            _container = _svc.GetBlobContainerClient(opts.ContainerName);
            _container.CreateIfNotExists(PublicAccessType.None); // contenedor privado
            _config = config;
        }

        public async Task<string> UploadAsync(Stream content, string fileName, string contentType, string extension, CancellationToken ct = default)
        {
            // Opcional: normalizar nombre
            var safeName = $"{Guid.NewGuid():N}{extension}".ToLowerInvariant();

            var blob = _container.GetBlobClient(safeName);
            var headers = new BlobHttpHeaders { ContentType = contentType };

            await blob.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers }, ct);

            // Agregar metadatos personalizados
            var metadata = new Dictionary<string, string>
            {
                { "customerId", "111111111"},
                { "total", "50"},
                { "items", "[\"Item_6\",\"Item_7\",\"Item_8\",\"Item_9\",\"Item_10\"]"}
            };

            await blob.SetMetadataAsync(metadata);

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

        public async Task<string> GenerateSASToken(string fileName)
        {
            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
            var safeName = $"{Guid.NewGuid():N}{ext}".ToLowerInvariant();

            BlobClient blobClient = _container.GetBlobClient(safeName);

            var sas = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
            };
            sas.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Create | BlobSasPermissions.Write);

            var accountName = _config.GetSection("AzureBlob")["AccountName"];
            var accountKey = _config.GetSection("Blob:AccountKey").Value;

            var token = sas.ToSasQueryParameters(
                new StorageSharedKeyCredential(accountName, accountKey) // Key1 sstorage account
            ).ToString();

            var sasUrl = $"{blobClient.Uri}?{token}";

            return sasUrl;
        }
    }
}
