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

public class CheckoutBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(
        CheckoutBasketCommand command,
        CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var basket = await dbContext.ShoppingCarts
                .Include(shoppingCart => shoppingCart.Items)
                .SingleOrDefaultAsync(shoppingCart =>
                    shoppingCart.UserName == command.BasketCheckout.UserName, cancellationToken)
                ?? throw new BasketNotFoundException(command.BasketCheckout.UserName);
            
            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();

            eventMessage.TotalPrice = basket.TotalPrice;

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccuredOn = DateTime.UtcNow
            };

            dbContext.OutboxMessages.Add(outboxMessage);
            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new CheckoutBasketResult(true);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            return new CheckoutBasketResult(false);
        }
    }
}