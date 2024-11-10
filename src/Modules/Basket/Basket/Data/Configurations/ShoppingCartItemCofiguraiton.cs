
namespace Basket.Data.Configurations;

public class ShoppingCartItemCofiguraiton : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.HasKey(item  => item.Id);

        builder.Property(item => item.ProductId)
            .IsRequired();

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.Color)
            .IsRequired();

        builder.Property(item => item.Price)
            .IsRequired();

        builder.Property(item => item.ProductName)
            .IsRequired();
    }
}
