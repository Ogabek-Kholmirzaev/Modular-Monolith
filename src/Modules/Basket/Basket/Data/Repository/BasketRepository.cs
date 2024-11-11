namespace Basket.Data.Repository;

public class BasketRepository(BasketDbContext dbContext) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(
        string userName,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.ShoppingCarts
            .Include(shoppingCart => shoppingCart.Items)
            .Where(shoppingCart => shoppingCart.UserName == userName);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        var basket = await query.SingleOrDefaultAsync(cancellationToken) 
            ?? throw new BasketNotFoundException(userName);

        return basket;
    }

    public async Task<ShoppingCart> CreateBasketAsync(
        ShoppingCart basket,
        CancellationToken cancellationToken = default)
    {
        dbContext.Add(basket);
        await dbContext.SaveChangesAsync(cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await GetBasketAsync(userName, false, cancellationToken);

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}