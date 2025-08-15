using Ecommerce.Api.Interfaces;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Interfaces;

namespace Ecommerce.Api.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public Images AddImage(string imageName)
        {
            Images image = new Images() { Name = imageName };
            _imageRepository.AddImage(image);

            return image;
        }

        public Images? GetImage(string imageName)
        {
            return _imageRepository.GetImage(imageName);
        }
    }
}
