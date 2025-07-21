using Microsoft.EntityFrameworkCore;
using RealTimeInventoryTracker.API.Entities;

namespace RealTimeInventoryTracker.API.Context;
public class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
}