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
using ExtensionMethods;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.ComponentModel;
using System.Windows.Markup;

namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for NewOrderPage.xaml
    /// </summary>

    internal partial class NewOrderPage : Page
    {
        private Inventory inventory = Inventory.GetInstance();
        private OrderManagement orderManagement = OrderManagement.GetInstance();
        private List<OrderItemsData> orderData = new List<OrderItemsData>();
        ICollectionView view;

        public NewOrderPage()
        {
            InitializeComponent();
            OnInit();
        }

        private void OnInit()
        {
            comboBoxItems.ItemsSource = inventory.GetStock();
            view = CollectionViewSource.GetDefaultView(orderData);
        }

        private void NewOrderSubmit_Click(object sender, RoutedEventArgs e)
        {
            orderManagement.AddOrder(txt_Receiver.Text, txt_Address.Text, orderData);
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }

        private void NewOrderCancel_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }

        private void NewSubstance_Click(object sender, RoutedEventArgs e)
        {
            var data = inventory.FindSubstance(Convert.ToInt32(comboBoxItems.SelectedValue));
            orderData.Add(new OrderItemsData(data.Id, data.Name, data.BatchNumber, data.Unit, Convert.ToInt32(txt_Amount.Text)));

            OrderView.ItemsSource = orderData;
            view.Refresh();
            
            txt_Amount.Clear();
            comboBoxItems.SelectedIndex = -1;
        }
    }
}

