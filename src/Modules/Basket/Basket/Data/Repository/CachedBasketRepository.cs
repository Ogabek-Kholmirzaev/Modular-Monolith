namespace Basket.Data.Repository;

public class CachedBasketRepository(IBasketRepository basketRepository) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(
        string userName,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        return await basketRepository.GetBasketAsync(userName, asNoTracking, cancellationToken);
    }

    public async Task<ShoppingCart> CreateBasketAsync(
        ShoppingCart shoppingCart,
        CancellationToken cancellationToken = default)
    {
        return await basketRepository.CreateBasketAsync(shoppingCart, cancellationToken);
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await basketRepository.DeleteBasketAsync(userName, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await basketRepository.SaveChangesAsync(cancellationToken);
    }
}