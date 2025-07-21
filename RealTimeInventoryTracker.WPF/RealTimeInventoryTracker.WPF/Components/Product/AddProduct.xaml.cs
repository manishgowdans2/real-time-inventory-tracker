using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RealTimeInventoryTracker.WPF.Components.Product
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public AddProduct()
        {
            InitializeComponent();

            AddProductButton.ButtonClicked += AddProductButton_Click;

            Exit.MouseLeftButtonDown += Exit_Clicked;
        }

        private async void AddProductButton_Click(object sender, System.EventArgs e)
        {
            string name = ProductNameTextBox.Text.Trim();
            string quantity = QuantityTextBox.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(quantity))
            {
                MessageBox.Show("Please enter both product name and quantity.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(quantity, out int quantityValue))
            {
                MessageBox.Show("Quantity must be a valid integer.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var product = new
            {
                Name = name,
                Quantity = quantityValue
            };

            try
            {
                string json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string apiUrl = "https://localhost:7063/api/product";

                // Use the instance of HttpClient instead of the class name
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ProductNameTextBox.Clear();
                    QuantityTextBox.Clear();
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error adding product: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error.Message);
            }
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            var backWindow = new Dashboard();
            backWindow.Show();
            this.Close();
        }
    }
}
