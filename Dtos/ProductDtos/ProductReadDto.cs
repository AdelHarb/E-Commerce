namespace ECommerce.Dtos.ProductDtos
{
    public class ProductReadDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public  IEnumerable<Category> Categories { get; set; }
    }
}