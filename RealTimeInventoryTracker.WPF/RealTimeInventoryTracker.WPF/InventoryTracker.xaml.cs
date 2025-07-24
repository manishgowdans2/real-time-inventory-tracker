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
using System.Windows.Shapes;

namespace RealTimeInventoryTracker.WPF
{
    /// <summary>
    /// Interaction logic for InventoryTracker.xaml
    /// </summary>
    public partial class InventoryTracker : Window
    {
        public InventoryTracker()
        {
            InitializeComponent();

            Exit.MouseLeftButtonDown += Exit_Clicked;
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            var backWindow = new MainWindow();
            backWindow.Show();
            this.Close();
        }
    }
}
