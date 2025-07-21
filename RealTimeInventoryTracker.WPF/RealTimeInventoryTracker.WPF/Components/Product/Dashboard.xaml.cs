using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();

            Exit.MouseLeftButtonDown += ExitMouseClicked;

            AddProductButton.ButtonClicked += AddProductButtonClicked;
            UpdateProductButton.ButtonClicked += UpdateProductButtonClicked;
            DeleteProductButton.ButtonClicked += DeleteProductButtonClicked;
        }

        private void ExitMouseClicked(object sender, MouseButtonEventArgs e)
        {
            var nextWindow = new MainWindow();
            nextWindow.Show();
            this.Close();
        }

        private void AddProductButtonClicked(object sender, EventArgs e)
        {
            var nextWindow = new AddProduct();
            nextWindow.Show();
            this.Close();
        }

        private void UpdateProductButtonClicked(object sender, EventArgs e)
        {
            var nextWindow = new UpdateProduct();
            nextWindow.Show();
            this.Close();
        }

        private void DeleteProductButtonClicked(object sender, EventArgs e)
        {
            var nextWindow = new DeleteProduct();
            nextWindow.Show();
            this.Close();
        }
    }
}
