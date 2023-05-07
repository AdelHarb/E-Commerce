namespace ECommerce.Dtos.CategoryDtos;

public class CategoryReadDto
{
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
}