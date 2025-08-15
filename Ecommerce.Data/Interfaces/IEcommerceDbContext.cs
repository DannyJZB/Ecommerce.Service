using Ecommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Interfaces
{
    public interface IEcommerceDbContext
    {
        DbSet<Products> Products { get; set; }
        DbSet<Images> Images { get; set; }
        DbSet<ProductImages> ProductImages { get; set; }
        int SaveChanges();
    }
}
