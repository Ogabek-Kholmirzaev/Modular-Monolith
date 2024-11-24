namespace Ordering.Orders.Features.CreateOrder;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderName is required");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("OrderName is required");
    }
}

public class CreateOrderHandler(OrderingDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddressDto = orderDto.ShippingAddress;
        var billingAddressDto = orderDto.BillingAddress;
        var paymentDto = orderDto.Payment;
        
        var shippingAddress = Address.Of(
            shippingAddressDto.FirstName,
            shippingAddressDto.LastName,
            shippingAddressDto.EmailAddress,
            shippingAddressDto.AddressLine,
            shippingAddressDto.Country,
            shippingAddressDto.State,
            shippingAddressDto.ZipCode);
        
        var billingAddress = Address.Of(
            billingAddressDto.FirstName,
            billingAddressDto.LastName,
            billingAddressDto.EmailAddress,
            billingAddressDto.AddressLine,
            billingAddressDto.Country,
            billingAddressDto.State,
            billingAddressDto.ZipCode);

        var payment = Payment.Of(
            paymentDto.CardName,
            paymentDto.CardNumber,
            paymentDto.Expiration,
            paymentDto.CVV,
            paymentDto.PaymentMethod);

        var order = Order.Create(
            Guid.NewGuid(),
            orderDto.CustomerId,
            $"{orderDto.OrderName}_{new Random().Next()}",
            shippingAddress,
            billingAddress,
            payment);
        
        orderDto.Items.ForEach(itemDto => order.Add(itemDto.ProductId, itemDto.Quantity, itemDto.Price));

        return order;
    }
}