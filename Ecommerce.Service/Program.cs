using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Interfaces;
using Ecommerce.Api.Services;
using Ecommerce.Data.Context;
using Ecommerce.Data.Interfaces;
using Ecommerce.Data.Repositories;
using Azure.Storage.Blobs;
using Ecommerce.Api.Blobs;

var builder = WebApplication.CreateBuilder(args);

//Setup vault
var kvUri = builder.Configuration["KeyVaultUri"];
if (!string.IsNullOrWhiteSpace(kvUri))
{
    builder.Configuration.AddAzureKeyVault(new Uri(kvUri!), new DefaultAzureCredential());
}

//Setup dbcontext
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IEcommerceDbContext, EcommerceDbContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

//Setup blob
var blobSection = builder.Configuration.GetSection("AzureBlob");
string? accountUrl = blobSection["AccountUrl"];
string? containerName = blobSection["ContainerName"];
string? conn = builder.Configuration["AzureBlob"];

builder.Services.AddSingleton(provider =>
{
    if (!string.IsNullOrWhiteSpace(conn))
    {
        return new BlobServiceClient(conn);
    }

    if (string.IsNullOrWhiteSpace(accountUrl))
        throw new InvalidOperationException("AzureBlob:AccountUrl no configurado.");

    return new BlobServiceClient(new Uri(accountUrl), new DefaultAzureCredential());
});

builder.Services.AddSingleton(new BlobContainerOptions(containerName!));
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
