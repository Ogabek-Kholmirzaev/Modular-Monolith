namespace Basket.Basket.Features.UpdateItemPriceInBasket;

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price, string ProductName)
    : ICommand<UpdateItemPriceInBasketResult>;

public record UpdateItemPriceInBasketResult(bool IsSuccess);

public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x => x.ProductName).NotEmpty().WithMessage("ProductName is required");
    }
}

public class UpdateItemPriceInBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(
        UpdateItemPriceInBasketCommand command,
        CancellationToken cancellationToken)
    {
        var items = await dbContext.ShoppingCartItems
            .Where(item => item.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
        {
            return new UpdateItemPriceInBasketResult(false);
        }
        
        foreach (var item in items)
        {
            item.Update(command.Price, command.ProductName);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateItemPriceInBasketResult(true);
    }
}