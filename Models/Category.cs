namespace ECommerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}