namespace ECommerce.Profiles;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        // Source -> Target
        CreateMap<Category, CategoryReadDto>();
    }
}