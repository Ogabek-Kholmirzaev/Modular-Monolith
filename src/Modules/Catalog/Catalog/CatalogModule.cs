using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        //add services to DI container

        services.AddDbContext<CatalogDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Database")
                ?? throw new ArgumentNullException("Database connection string is null");

            options.UseNpgsql(connectionString);
            options.AddInterceptors(new AuditableEntityInterceptor());
        });

        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        //configure the http request pipeline

        app.UseMigration<CatalogDbContext>();

        return app;
    }
}
