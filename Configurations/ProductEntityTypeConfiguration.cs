namespace ECommerce.Configurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(10);
        builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");

        builder.HasMany(p => p.Categories)
        .WithMany(c => c.Products).
        UsingEntity<ProductCategory>(
            j => j
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId),
            j => j
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId),
            j =>
            {
                j.Property(pc => pc.CreatedAt).HasDefaultValueSql("GETDATE()");
                j.HasKey(pc => new { pc.ProductId, pc.CategoryId });
            }
        );
    }
}