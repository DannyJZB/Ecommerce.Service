using Ecommerce.Data.Entities;

namespace Ecommerce.Api.Interfaces
{
    public interface IImageService
    {
        Images AddImage(string imageName);
        Images? GetImage(string imageName);
    }
}
