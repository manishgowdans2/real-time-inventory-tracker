namespace RealTimeInventoryTracker.API.Services.Product;
public interface IProductService
{
    Task AddProduct(Entities.Product product, CancellationToken cancellationToken);
    Task<bool> UpdateProduct(Entities.Product product, CancellationToken cancellationToken);
    Task<bool> DeleteProduct(int productId, CancellationToken cancellationToken);
}
