using Ecommerce.Data.Entities;

namespace Ecommerce.Data.Interfaces
{
    public interface IImageRepository
    {
        Images AddImage(Images image);
        Images? GetImage(string imageName);
    }
}
