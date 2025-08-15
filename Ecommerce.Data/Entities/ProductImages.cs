namespace Ecommerce.Data.Entities
{
    public class ProductImages
    {
        public int ProductId { get; set; }
        public int ImageId { get; set; }
        public Products Product { get; set; } = null!;
        public Images Image { get; set; } = null!;
    }
}
