using RealTimeInventoryTracker.WPF.Components.Product;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealTimeInventoryTracker.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            EnterButton.ButtonClicked += EnterButtonClicked;
            ManageProducts.MouseLeftButtonDown += ManageProductsMouseClicked;
        }

        //private void EnterButtonClicked(object sender, EventArgs e)
        //{
        //    var nextWindow = new AddProduct();
        //    nextWindow.Show();
        //    this.Close();
        //}

        private void ManageProductsMouseClicked(object sender, MouseButtonEventArgs e)
        {
            var nextWindow = new Dashboard();
            nextWindow.Show();
            this.Close();
        }

        private void EnterButtonClicked(object sender, EventArgs e)
        {
            var nextWindow = new InventoryTracker();
            nextWindow.Show();
        }
    }
}
