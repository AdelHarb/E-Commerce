namespace ECommerce.Configurations;

public class CartEntityTypeConfiguration: IEntityTypeConfiguration<UserProductsCart>
{
    public void Configure(EntityTypeBuilder<UserProductsCart> builder)
    {
        builder.HasOne(e => e.User)
            .WithMany(e => e.UsersProductsCarts)
            .HasForeignKey(e => e.UserId);

        builder.HasOne(e => e.Product)
            .WithMany(e => e.UsersProductsCarts)
            .HasForeignKey(e => e.ProductId);
        
        builder.HasKey( e => new { e.UserId, e.ProductId });
    }
}