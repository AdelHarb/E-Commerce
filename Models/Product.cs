namespace ECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public virtual IEnumerable<Category> Categories { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; } = string.Empty;
        public IEnumerable<Review> Reviews { get; set; } = null!;

        public IEnumerable<ProductCategory> ProductCategories { get; set; } = null!;
        public IEnumerable<UserProductsCart> UsersProductsCarts { get; set; } = new HashSet<UserProductsCart>();
        public IEnumerable<OrderProductDetails> OrderProductDetails { get; set; } = new List<OrderProductDetails>();
    }

    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}