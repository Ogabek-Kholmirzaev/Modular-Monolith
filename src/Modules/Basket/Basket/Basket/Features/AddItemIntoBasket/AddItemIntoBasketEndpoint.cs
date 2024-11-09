namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(string UserName, ShoppingCartItemDto ShoppingCartItem);

public record AddItemIntoBasketResponse(Guid Id);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/basket/{userName}/items",
            async ([FromRoute] string userName, [FromBody] AddItemIntoBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<AddItemIntoBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<AddItemIntoBasketResponse>();

                return Results.Created($"/basket/{response.Id}", response);
            });
    }
}