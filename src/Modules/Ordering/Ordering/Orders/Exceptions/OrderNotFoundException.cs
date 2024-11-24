namespace Ordering.Orders.Exceptions;

public class OrderNotFoundException(Guid id) : NotFoundException("Order", id)
{
}