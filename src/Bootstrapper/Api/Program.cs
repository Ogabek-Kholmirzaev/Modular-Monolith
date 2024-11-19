var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

//add services to the container

var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly);
builder.Services.AddMediatRWithAssemblies(catalogAssembly, basketAssembly);

builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis"));

builder.Services.AddMassTransitWithAssemblies(catalogAssembly, basketAssembly);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//configure the HTTP request pipelines
app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });

app
    .UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();

app.Run();
