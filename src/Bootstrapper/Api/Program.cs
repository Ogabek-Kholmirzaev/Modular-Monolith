var builder = WebApplication.CreateBuilder(args);

//add services to the container
builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//configure the HTTP request pipelines
app.MapCarter();

app
    .UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();

app.UseExceptionHandler(options => { });

app.Run();
