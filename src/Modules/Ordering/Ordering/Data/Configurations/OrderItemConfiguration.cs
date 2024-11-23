namespace Ordering.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(item => item.Id);
        builder.Property(item => item.ProductId).IsRequired();
        builder.Property(item => item.Quantity).IsRequired();
        builder.Property(item => item.Price).IsRequired(); 
    }
}