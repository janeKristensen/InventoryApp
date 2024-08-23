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
using ExtensionMethods;


namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
    {

        OrderManagement orderManagement = OrderManagement.GetInstance();

        public MainWindow()
        {
            InitializeComponent();
            OnInit();
        }

        public void OnInit()
        {
            MainFrame.Navigate(new Uri("SubstanceOverviewPage.xaml", UriKind.Relative));
        }

        private void BtnNavigateNew_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("NewSubstancePage.xaml"); 
        }

        private void BtnNavigateNewOrder_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("NewOrderPage.xaml");
        }

        private void BtnPrintOrders_Click(object sender, RoutedEventArgs e)
        {
            orderManagement.PrintOrders();
        }

        private void BtnNavigateViewOrders_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("ViewOrderPage.xaml");
        }

        private void BtnNavigateViewSubstances_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }
    }
}
