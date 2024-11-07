namespace Basket.Data.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(shoppingCart => shoppingCart.Id);

        builder.HasIndex(shoppingCart => shoppingCart.UserName)
            .IsUnique();

        builder.Property(shoppingCart => shoppingCart.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(shoppingCart => shoppingCart.Items)
            .WithOne()
            .HasForeignKey(items => items.ShoppingCartId);
    }
}
