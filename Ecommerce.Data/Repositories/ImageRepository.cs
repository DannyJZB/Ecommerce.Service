using Ecommerce.Data.Entities;
using Ecommerce.Data.Interfaces;

namespace Ecommerce.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IEcommerceDbContext _ecommerceDbContext;

        public ImageRepository(IEcommerceDbContext ecommerceDbContext)
        {
            _ecommerceDbContext = ecommerceDbContext;
        }

        public Images AddImage(Images image)
        {
            _ecommerceDbContext.Images.Add(image);
            _ecommerceDbContext.SaveChanges();

            return image;
        }

        public Images? GetImage(string imageName)
        {
            return _ecommerceDbContext.Images.Where(x => x.Name!.Equals(imageName)).OrderByDescending(c => c.ImageId).FirstOrDefault();
        }
    }
}
