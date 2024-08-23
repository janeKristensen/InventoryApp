using InventoryManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
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
using ExtensionMethods;

namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for ViewOrderPage.xaml
    /// </summary>
    /// 
    internal partial class ViewOrderPage : Page
    {
        private readonly MainWindow _main = Application.Current.MainWindow as MainWindow;

        public ViewOrderPage()
        {
            InitializeComponent();
            OnInit();
        }

        private void OnInit()
        {
            using (var db = new SubstanceContext())
            {
                var orders = db.Orders.ToList();
                OrderListView.ItemsSource = orders;
            }       
        }

        private void GoToOrderPage(object sender, MouseButtonEventArgs e)
        {
            Order item = OrderListView.SelectedItem as Order;
            if (item != null)
            {
                OrderPage p = new OrderPage(item);
                _main.MainFrame.NavigationService.Navigate(p);
            }
            
        }
    }
}
