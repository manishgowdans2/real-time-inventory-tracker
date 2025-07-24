using Microsoft.AspNetCore.SignalR.Client;
using RealTimeInventoryTracker.WPF.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealTimeInventoryTracker.WPF
{
    public class InventoryViewModel
    {
        public ObservableCollection<Product> Products { get; private set; }

        public InventoryViewModel()
        {
            Products = new ObservableCollection<Product>();

            var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7063/inventoryHub")
                .Build();

            hubConnection.On<int, string, int>("ReceiveInventoryUpdate", (id, name, quantity) =>
            {
                Console.WriteLine($"Received update for Product ID {id}, Product Name {name}, Quantity {quantity}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var product = Products.FirstOrDefault(p => p.Id == id);
                    if (product != null)
                    {
                        product.Name = name;
                        product.Quantity = quantity;
                    }
                    else
                    {
                        Products.Add(new Product { Id = id, Name = name, Quantity = quantity });
                    }
                });
            });

            hubConnection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show("Error connecting to the SignalR hub: " + task.Exception?.GetBaseException().Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}
