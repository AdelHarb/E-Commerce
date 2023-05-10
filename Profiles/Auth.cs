namespace ECommerce.Profiles;

public class Auth : Profile
{
    public Auth()
    {
        // Source -> Target
        CreateMap<RegisterDto, ApplicationUser>();
    }
}