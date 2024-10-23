using Basket;
using Catalog;
using Ordering;

var builder = WebApplication.CreateBuilder(args);

//add services to the container
builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();

//configure the HTTP request pipelines
app
    .UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();

app.Run();
