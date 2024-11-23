using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(order => order.Id);
        builder.Property(order => order.CustomerId);
        builder.HasIndex(order => order.OrderName).IsUnique();

        builder.Property(order => order.OrderName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(order => order.Items)
            .WithOne()
            .HasForeignKey(item => item.Id);

        builder.ComplexProperty(order => order.ShippingAddress, propertyBuilder =>
        {
            propertyBuilder.Property(address => address.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            propertyBuilder.Property(address => address.LastName)
                .HasMaxLength(50)
                .IsRequired();

            propertyBuilder.Property(address => address.EmailAddress)
                .HasMaxLength(50);

            propertyBuilder.Property(address => address.AddressLine)
                .HasMaxLength(180)
                .IsRequired();

            propertyBuilder.Property(address => address.Country)
                .HasMaxLength(50);

            propertyBuilder.Property(address => address.State)
                .HasMaxLength(50);
            
            propertyBuilder.Property(address => address.ZipCode)
                .HasMaxLength(6)
                .IsRequired();
        });
        
        builder.ComplexProperty(order => order.BillingAddress, propertyBuilder =>
        {
            propertyBuilder.Property(address => address.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            propertyBuilder.Property(address => address.LastName)
                .HasMaxLength(50)
                .IsRequired();

            propertyBuilder.Property(address => address.EmailAddress)
                .HasMaxLength(50);

            propertyBuilder.Property(address => address.AddressLine)
                .HasMaxLength(180)
                .IsRequired();

            propertyBuilder.Property(address => address.Country)
                .HasMaxLength(50);

            propertyBuilder.Property(address => address.State)
                .HasMaxLength(50);
            
            propertyBuilder.Property(address => address.ZipCode)
                .HasMaxLength(6)
                .IsRequired();
        });

        builder.ComplexProperty(order => order.Payment, propertyBuilder =>
        {
            propertyBuilder.Property(payment => payment.CardName)
                .HasMaxLength(50);

            propertyBuilder.Property(payment => payment.CardNumber)
                .HasMaxLength(24)
                .IsRequired();

            propertyBuilder.Property(payment => payment.Expiration)
                .HasMaxLength(10);

            propertyBuilder.Property(payment => payment.CVV)
                .HasMaxLength(3);

            propertyBuilder.Property(payment => payment.PaymentMethod);
        });
    }
}