namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
    : ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(Guid Id);

public class RemoveItemFromBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(
        RemoveItemFromBasketCommand command,
        CancellationToken cancellationToken)
    {
        var shoppingCart = await dbContext.ShoppingCarts
            .Include(shoppingCart => shoppingCart.Items)
            .SingleOrDefaultAsync(shoppingCart => shoppingCart.UserName == command.UserName, cancellationToken)
            ?? throw new BasketNotFoundException(command.UserName);
        
        shoppingCart.RemoveItem(command.ProductId);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}