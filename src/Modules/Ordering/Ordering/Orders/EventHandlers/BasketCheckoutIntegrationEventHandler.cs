using Ordering.Orders.Features.CreateOrder;

namespace Ordering.Orders.EventHandlers;

public class BasketCheckoutIntegrationEventHandler(
    ISender sender,
    ILogger<BasketCheckoutIntegrationEventHandler> logger)
    : IConsumer<BasketCheckoutIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        logger.LogInformation($"Integration Event handled: {context.Message.GetType().Namespace}");

        var createOrderCommand = MapToCreateOrderCommand(context.Message);

        await sender.Send(createOrderCommand);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutIntegrationEvent message)
    {
        var addressDto = new AddressDto(
            message.FirstName,
            message.LastName,
            message.EmailAddress,
            message.AddressLine,
            message.Country,
            message.State,
            message.ZipCode);

        var paymentDto = new PaymentDto(
            message.CardName,
            message.CardNumber,
            message.Expiration,
            message.CVV,
            message.PaymentMethod);

        var orderId = Guid.NewGuid();

        //TODO: need to configure products (order items)
        
        var orderDto = new OrderDto(
            orderId,
            message.CustomerId,
            message.UserName,
            addressDto,
            addressDto,
            paymentDto,
            []);

        return new CreateOrderCommand(orderDto);
    }
}