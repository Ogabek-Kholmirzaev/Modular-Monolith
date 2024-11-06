using Shared.DDD;

namespace Basket.Basket.Models;

public class ShoppingCart : Aggregate<Guid>
{
    private readonly List<ShoppingCartItem> _items = new();

    public string UserName { get; private set; } = default!;
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}
