using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.Data.JsonConverters;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Data.Repository;

public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(
        string userName,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new ShoppingCartConverter(),
                    new ShoppingCartItemConverter()
                }
            };
            
            return await basketRepository.GetBasketAsync(userName, asNoTracking, cancellationToken);
        }

        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        var basket = await basketRepository.GetBasketAsync(userName, asNoTracking, cancellationToken);

        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> CreateBasketAsync(
        ShoppingCart shoppingCart,
        CancellationToken cancellationToken = default)
    {
        await basketRepository.CreateBasketAsync(shoppingCart, cancellationToken);
        await cache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart), cancellationToken);

        return shoppingCart;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        await basketRepository.DeleteBasketAsync(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        var result = await basketRepository.SaveChangesAsync(userName, cancellationToken);

        if (userName != null)
        {
            await cache.RemoveAsync(userName, cancellationToken);
        }
        
        return result;
    }
}