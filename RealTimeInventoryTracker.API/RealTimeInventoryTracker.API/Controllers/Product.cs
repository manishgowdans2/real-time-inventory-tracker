using Microsoft.AspNetCore.Mvc;
using RealTimeInventoryTracker.API.Services.Product;

namespace RealTimeInventoryTracker.API.Controllers;

[Controller]
[Route("/api")]
public class Product(IProductService productService) : ControllerBase
{
    [HttpPost]
    [Route("product")]
    public async Task<ActionResult> PostProduct([FromBody] ProductRequest request, CancellationToken cancellationToken = default)
    {
        if(request == null)
        {
            return BadRequest("Add product payload");
        }

        await productService.AddProduct(new Entities.Product
        {
            Name = request.Name,
            Quantity = request.Quantity
        }, cancellationToken);

        return Ok("Product Succesfully Added");
    }

    [HttpPut]
    [Route("product/{productId:int}")]
    public async Task<ActionResult> PutProduct(int productId, [FromBody] ProductUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var updated = await productService.UpdateProduct(new Entities.Product
        {
            Id = productId,
            Quantity = request.Quantity
        }, cancellationToken);
        if(!updated)
        {
            return NotFound("Product not found");
        }
        return Ok("Product Succesfully Updated");
    }

    [HttpDelete]
    [Route("product/{productId:int}")]
    public async Task<ActionResult> DeleteProduct(int productId, CancellationToken cancellationToken = default)
    {
        if(productId <= 0)
        {
            return BadRequest("Invalid product ID");
        }
        var isDeleted = await productService.DeleteProduct(productId, cancellationToken);
        if(isDeleted)
        {
            return NotFound("Product not found");
        }
        return Ok("Product Succesfully Deleted");
    }
}

public class ProductRequest
{
    public string Name { get; set; } = "Unknown Product";
    public int Quantity { get; set; }
}

public class  ProductUpdateRequest
{
    public int Quantity { get; set; }
}
