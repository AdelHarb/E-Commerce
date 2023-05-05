namespace ECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public virtual IEnumerable<Category> Categories { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}