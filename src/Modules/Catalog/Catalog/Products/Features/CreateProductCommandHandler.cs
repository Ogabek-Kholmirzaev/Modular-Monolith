using MediatR;

namespace Catalog.Products.Features;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : IRequest<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //TODO: need to implement business logic to create a new product
        throw new NotImplementedException();
    }
}
