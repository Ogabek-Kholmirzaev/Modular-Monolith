namespace Basket.Basket.Features.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto cannot be null");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("BasketCheckoutDto is required");
    }
}

public class CheckoutBasketHandler(IBasketRepository basketRepository, IBus bus)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(
        CheckoutBasketCommand command,
        CancellationToken cancellationToken)
    {
        var basket = await basketRepository
            .GetBasketAsync(command.BasketCheckout.UserName, true, cancellationToken);

        var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();

        eventMessage.TotalPrice = basket.TotalPrice;

        await bus.Publish(eventMessage, cancellationToken);
        await basketRepository.DeleteBasketAsync(command.BasketCheckout.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}