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
    public partial class DeleteProduct : Window
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public DeleteProduct()
        {
            InitializeComponent();

            DeleteProductButton.ButtonClicked += DeleteProductButton_Click;

            Exit.MouseLeftButtonDown += Exit_Clicked;
        }

        private async void DeleteProductButton_Click(object sender, System.EventArgs e)
        {
            string id = ProductIdTextBox.Text.Trim();

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Please enter product id.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(id, out int productId))
            {
                MessageBox.Show("Quantity must be a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string apiUrl = $"https://localhost:7063/api/product/{productId}";

                // Use the instance of HttpClient instead of the class name
                var response = await httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ProductIdTextBox.Clear();
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error deleting product: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
