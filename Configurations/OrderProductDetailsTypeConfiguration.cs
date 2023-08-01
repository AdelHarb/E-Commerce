namespace ECommerce.Configurations;

public class OrderProductDetailsTypeConfiguration : IEntityTypeConfiguration<OrderProductDetails>
{
    public void Configure(EntityTypeBuilder<OrderProductDetails> builder)
    {
        builder.Property(o => o.Quantity).IsRequired();

        builder.HasOne(o => o.Order)
                .WithMany(o => o.OrderProductDetails)
                .HasForeignKey( o => o.OrderId);

        builder.HasOne(e => e.Product)
                .WithMany(e => e.OrderProductDetails)
                .HasForeignKey(e => e.ProductId);
            
        builder.HasKey( e => new { e.OrderId, e.ProductId });

    }
}