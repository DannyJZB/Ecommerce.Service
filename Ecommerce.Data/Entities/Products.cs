namespace Ecommerce.Data.Entities
{
    public class Products
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public string? Color { get; set; }
        public virtual ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
    }
}
