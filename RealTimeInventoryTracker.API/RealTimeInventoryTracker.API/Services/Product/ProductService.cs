using Microsoft.EntityFrameworkCore;
using RealTimeInventoryTracker.API.Context;

namespace RealTimeInventoryTracker.API.Services.Product;
public class ProductService(InventoryContext context) : IProductService
{
    public async Task AddProduct(Entities.Product product, CancellationToken cancellationToken)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateProduct(Entities.Product product, CancellationToken cancellationToken)
    {
        var productResponse = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == product.Id, cancellationToken);
        if (productResponse == null)
        {
            return false;
        }
        product.Name = productResponse.Name;
        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteProduct(int productId, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync([productId], cancellationToken);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync(cancellationToken);

            return false;
        }

        return true;
    }
}
