using InventoryManagement;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;



namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Singleton instance of inventory and Ordermanagement
        Inventory inventory = Inventory.GetInstance();
        OrderManagement orderManagement = OrderManagement.GetInstance();

        public MainWindow()
        {
            InitializeComponent();

            using (var db = new SubstanceContext())
            {
                var data = db.ReferenceSubstances.ToList();
                DatabaseView.ItemsSource = data;
            };
        }

        private void BtnNavigateNew_Click(object sender, RoutedEventArgs e)
        {
            NavigationWindow window = new NavigationWindow();
            window.Source = new Uri("NewSubstancePage.xaml", UriKind.Relative);
            window.ShowsNavigationUI = false;
            Uri iconUri = new Uri("D:\\Visual Studio stuff\\Projekts\\InventoryApp\\InventoryApp\\desktopfolder.ico", UriKind.RelativeOrAbsolute);
            window.Icon = BitmapFrame.Create(iconUri);
            window.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void OnInit(object sender, EventArgs e)
        {
            
                
        }

        private void NameCM_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
