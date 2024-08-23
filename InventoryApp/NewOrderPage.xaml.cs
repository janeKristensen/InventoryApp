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

namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for NewOrderPage.xaml
    /// </summary>

    internal partial class NewOrderPage : Page
    {
        private List<(int, int)> orderItems = new List<(int, int)>();

        private List<OrderItemsData> orderData = new List<OrderItemsData>();
        ICollectionView view;

        public NewOrderPage()
        {
            InitializeComponent();
            OnInit();
        }

        private void OnInit()
        {
            using (var db = new SubstanceContext())
            {
                var substanceItems = db.ReferenceSubstances.ToList();
                comboBoxItems.ItemsSource = substanceItems;
            };

            view = CollectionViewSource.GetDefaultView(orderData);
        }

        private void NewOrderSubmit_Click(object sender, RoutedEventArgs e)
        {
            OrderManagement orderManagement = OrderManagement.GetInstance();
            orderManagement.AddOrder(txt_Receiver.Text, txt_Address.Text, orderItems);
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }

        private void NewOrderCancel_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }

        private void NewSubstance_Click(object sender, RoutedEventArgs e)
        {
            (int, int) t = (Convert.ToInt32(comboBoxItems.SelectedValue), Convert.ToInt32(txt_Amount.Text));
            orderItems.Add(t);
            using (var db = new SubstanceContext())
            {
                var data = db.ReferenceSubstances.Where(x => x.Id == Convert.ToInt32(comboBoxItems.SelectedValue)).First();
                OrderItemsData order_items = new OrderItemsData(data.Id, data.Name, data.BatchNumber, data.Unit, txt_Amount.Text);
                orderData.Add(order_items);
            };

            OrderView.ItemsSource = orderData;
            view.Refresh();
            
            txt_Amount.Clear();
            comboBoxItems.SelectedIndex = -1;
        }
    }
}
