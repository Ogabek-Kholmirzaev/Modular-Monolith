var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

//add services to the container

var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(catalogAssembly, basketAssembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblies([catalogAssembly, basketAssembly]);

builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

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
