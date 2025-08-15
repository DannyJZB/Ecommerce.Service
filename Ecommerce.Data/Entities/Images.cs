namespace Ecommerce.Data.Entities
{
    public class Images
    {
        public int ImageId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
    }
}
