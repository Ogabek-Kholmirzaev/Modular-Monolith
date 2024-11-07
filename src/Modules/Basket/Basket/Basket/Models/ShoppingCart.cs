namespace Basket.Basket.Models;

public class ShoppingCart : Aggregate<Guid>
{
    private readonly List<ShoppingCartItem> _items = new();

    public string UserName { get; private set; } = default!;
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

    public static ShoppingCart Create(Guid id, string userName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        var shoppingCart = new ShoppingCart
        {
            Id = id,
            UserName = userName
        };

        return shoppingCart;
    }

    public void AddItem(
        Guid productId,
        int quantity,
        string color,
        decimal price,
        string productName)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItem = Items.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _items.Add(new ShoppingCartItem(Id, productId, quantity, color, price, productName));
        }
    }

    public void RemoveItem(Guid productId)
    {
        var existingItem = Items.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }
    }
}
