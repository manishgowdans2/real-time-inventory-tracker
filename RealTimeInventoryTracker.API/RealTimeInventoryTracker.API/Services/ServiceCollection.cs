using RealTimeInventoryTracker.API.Services.Product;

namespace RealTimeInventoryTracker.API.Services;
public static class ServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IProductService, ProductService>();
        return services;
    }
}