using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using RealTimeInventoryTracker.API.Hubs;
using System.Text.Json;

namespace RealTimeInventoryTracker.API.Kafka;

public class Consumer : BackgroundService
{
    private readonly IHubContext<InventoryHub> _hubContext;
    private readonly IConsumer<Ignore, string> _kafkaConsumer;

    public Consumer(IHubContext<InventoryHub> hubContext)
    {
        _hubContext = hubContext;

        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "inventory-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _kafkaConsumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _kafkaConsumer.Subscribe("cdc.public.Products");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumerResult = _kafkaConsumer.Consume(stoppingToken);

                if (consumerResult.Message.Value != null)
                {
                    using (var doc = JsonDocument.Parse(consumerResult.Message.Value))
                    {
                        doc.RootElement.TryGetProperty("payload", out var payload);

                        if (payload.ValueKind == JsonValueKind.Null)
                        {
                            continue;
                        }

                        if (payload.TryGetProperty("after", out var afterState))
                        {
                            if(afterState.Equals(JsonValueKind.Null))
                            {
                                Console.WriteLine("Received null after state, skipping.");
                                continue;
                            }
                            var id = afterState.GetProperty("Id").GetInt32();
                            var name = afterState.GetProperty("Name").GetString();
                            var quantity = afterState.GetProperty("Quantity").GetInt32();

                            Console.WriteLine($"Received update for Product ID {id}, Product Name {name}, Quantity {quantity}");

                            await _hubContext.Clients.All.SendAsync("ReceiveInventoryUpdate", id, name, quantity)
                                .ContinueWith(task =>
                                {
                                    if (task.IsFaulted)
                                    {
                                        Console.WriteLine($"Error sending update to clients: {task.Exception?.GetBaseException().Message}");
                                    }
                                }, stoppingToken); ;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            // Log the exception (consider using a logging framework)
            Console.WriteLine($"Error in Kafka consumer: {e.Message}");
            
        }
    }
}
