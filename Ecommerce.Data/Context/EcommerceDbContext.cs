using Ecommerce.Data.Entities;
using Ecommerce.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Context
{
    public class EcommerceDbContext : DbContext, IEcommerceDbContext
    {
        public EcommerceDbContext() { }

        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options) { }

        public DbSet<Products> Products { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            //var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            //if (string.Equals(env, "Development", StringComparison.OrdinalIgnoreCase))
            //{
            //    var basePath = Path.Combine(Directory.GetCurrentDirectory());
            //    var config = new ConfigurationBuilder()
            //    .SetBasePath(basePath)
            //    .AddJsonFile("appsettings.json", optional: false)
            //    .AddJsonFile($"appsettings.{env}.json", optional: true)
            //    .AddEnvironmentVariables()
            //    .Build();

            //    var conn = config.GetConnectionString("Default");

            optionsBuilder.UseSqlServer("Server=tcp:ecommersesserver.database.windows.net,1433;Initial Catalog=ecommerce;Persist Security Info=False;User ID=Danny_199512;Password=Ecommerce@XXXX;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductImages>(e =>
            {
                e.ToTable("ProductImages"); // nombre de la tabla de unión
                e.HasKey(x => new { x.ProductId, x.ImageId });

                e.HasOne(x => x.Product)
                 .WithMany(p => p.ProductImages)
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Image)
                 .WithMany(i => i.ProductImages)
                 .HasForeignKey(x => x.ImageId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => x.ImageId); // opcional, recomendado
            });

            // 1) Asegurar PK por el nombre plural de la clase
            modelBuilder.Entity<Images>(e =>
            {
                e.HasKey(i => i.ImageId);
                e.Property(i => i.ImageId)
                    .ValueGeneratedOnAdd(); // IDENTITY
            });

            modelBuilder.Entity<Products>(e =>
            {
                e.HasKey(p => p.ProductId);
                e.Property(p => p.ProductId)
                    .ValueGeneratedOnAdd(); // IDENTITY
            });
        }
    }
}
