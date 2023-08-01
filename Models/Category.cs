namespace ECommerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual IEnumerable<Product> Products { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; } = null!;
    }
}