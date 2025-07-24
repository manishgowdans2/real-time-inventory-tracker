using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using RealTimeInventoryTracker.API.Context;
using RealTimeInventoryTracker.API.Kafka;
using RealTimeInventoryTracker.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddDbContext<InventoryContext>(
    service => service.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//try
//{
//    builder.Services.AddSingleton<IConsumer<Ignore, string>>(provider =>
//    {
//        var config = new ConsumerConfig
//        {
//            BootstrapServers = "localhost:9092",
//            GroupId = "inventory-group-4",
//            AutoOffsetReset = AutoOffsetReset.Earliest
//        };
//        return new ConsumerBuilder<Ignore, string>(config).Build();
//    });

//}

//catch (Exception ex)
//{
//    Console.WriteLine($"Error configuring Kafka consumer: {ex.Message}");
//    throw;
//}

builder.Services.AddApplication();


try
{
    builder.Services.AddHostedService<Consumer>();
}
catch (Exception ex)
{
    Console.WriteLine($"Error adding Kafka consumer service: {ex.Message}");
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<RealTimeInventoryTracker.API.Hubs.InventoryHub>("/inventoryHub");

try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Error running the application: {ex.Message}");
    
}
