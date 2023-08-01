namespace ECommerce.Configurations;

public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {

        builder.ToTable("Users");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.FirstName).HasMaxLength(10);
        builder.Property(p => p.LastName).HasMaxLength(10);
        builder.Property(p => p.Address).HasMaxLength(100);
        // builder.Property(p => p.ProfilePicture).HasColumnType("image");

        builder.Property(p => p.FirstName).IsRequired();
        builder.Property(p => p.LastName).IsRequired();
        builder.Property(p => p.Address).IsRequired();
        // builder.Property(p => p.ProfilePicture).IsRequired();


    }
}