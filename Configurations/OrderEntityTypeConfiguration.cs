namespace ECommerce.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.CreatedAt).HasDefaultValue("GETDATE()");
        builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.SetNull);

    }
}