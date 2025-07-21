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
    /// Interaction logic for UpdateProduct.xaml
    /// </summary>
    public partial class UpdateProduct : Window
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public UpdateProduct()
        {
            InitializeComponent();

            Exit.MouseLeftButtonDown += Exit_Clicked;

            UpdateProductButton.ButtonClicked += UpdateProductButton_Click;
        }

        private async void UpdateProductButton_Click(object sender, System.EventArgs e)
        {
            string id = ProductIdTextBox.Text.Trim();
            string quantity = QuantityTextBox.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(quantity))
            {
                MessageBox.Show("Please enter both product id and quantity.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(quantity, out int quantityValue))
            {
                MessageBox.Show("Quantity must be a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(id, out int productId))
            {
                MessageBox.Show("Quantity must be a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var product = new
            {
                Quantity = quantityValue
            };

            try
            {
                string json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string apiUrl = $"https://localhost:7063/api/product/{productId}";

                // Use the instance of HttpClient instead of the class name
                var response = await httpClient.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ProductIdTextBox.Clear();
                    QuantityTextBox.Clear();
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error updating product: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
