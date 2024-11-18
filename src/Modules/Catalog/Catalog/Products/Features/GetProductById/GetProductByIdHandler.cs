namespace Catalog.Products.Features.GetProductById;

public class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(product => product.Id == query.Id, cancellationToken)
            ?? throw new ProductNotFoundException(query.Id);

        var productDto = product.Adapt<ProductDto>();

        return new GetProductByIdResult(productDto);
    }
}
