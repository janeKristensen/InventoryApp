using ExtensionMethods;
using InventoryManagement;
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

namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for SubstanceOverviewPage.xaml
    /// </summary>
    internal partial class SubstanceOverviewPage : Page
    {
        private Inventory _inventory = Inventory.GetInstance();
        public SubstanceOverviewPage()
        {
            InitializeComponent();
            OnInit();
        }

        public void OnInit()
        {
            DatabaseView.ItemsSource = _inventory.GetStock();
        }

        private void NavigateItemsPage(object sender, MouseButtonEventArgs e)
        {
            Substance item = DatabaseView.SelectedItem as Substance;
            if (item != null)
            {
                this.NavigationService.Navigate(new ItemPage(item));
            } 
        }
    }
}
