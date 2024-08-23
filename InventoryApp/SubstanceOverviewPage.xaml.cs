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
        private readonly MainWindow _main = Application.Current.MainWindow as MainWindow;

        public SubstanceOverviewPage()
        {
            InitializeComponent();
            OnInit();
        }

        public void OnInit()
        {

            using (var db = new SubstanceContext())
            {
                var data = db.ReferenceSubstances.ToList();
                DatabaseView.ItemsSource = data;
            };
        }

        private void NavigateItemsPage(object sender, MouseButtonEventArgs e)
        {
            Substance item = DatabaseView.SelectedItem as Substance;
            if (item != null)
            {
                ItemPage p = new ItemPage(item);
                this.NavigationService.Navigate(p);
            } 
        }

       
    }
}
