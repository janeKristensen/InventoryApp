using InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ExtensionMethods;
using System.Windows.Markup;
using System.Net;


namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    internal partial class OrderPage : Page
    {
        private Order _order;
        private OrderManagement _orderManagement = OrderManagement.GetInstance();

        public OrderPage()
        {
            InitializeComponent();
        }

        public OrderPage(Order order) : this()
        {
            this._order = order;
            this.Loaded += new RoutedEventHandler(OrderPage_Loaded);
        }

        void OrderPage_Loaded(object sender, RoutedEventArgs e)
        {
            OnInit();
        }

        public void OnInit()
        {
            this.txt_OrderID.Text = _order.Id.ToString();
            this.txt_Receiver.Text = _order.Receiver;
            this.txt_Address.Text = _order.Address;

            OrderDetailView.ItemsSource = _orderManagement.GetOrderData(_order);
        }

        private void PrintOrder(object sender, RoutedEventArgs e)
        {
            _orderManagement.PrintOrder(_order);
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            _order.Receiver = txt_Receiver.Text;
            _order.Address = txt_Address.Text;
            _orderManagement.UpdateOrder(_order);

            txt_Receiver.IsEnabled = false;
            txt_Address.IsEnabled = false;
            OrderDetailView.IsEnabled = false;
            Btn_EditOrder.Content = "Edit";
            Btn_EditOrder.Click += new RoutedEventHandler(EditOrder);
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            txt_Receiver.IsEnabled = true;
            txt_Address.IsEnabled = true;
            OrderDetailView.IsEnabled = true;
            Btn_EditOrder.Content = "Save";
            Btn_EditOrder.Click += new RoutedEventHandler(SaveChanges);
        }

        private void OrderDetailView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (OrderItemsData)OrderDetailView.SelectedItem;
            ExtensionMethodsPages.NavigateToPage(new OrderItemPage(item, _order));
        }

        private void CancelOrder(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("ViewOrderPage.xaml");
        }

        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            _orderManagement.DeleteOrder(_order);
            ExtensionMethodsPages.NavigateTo("ViewOrderPage.xaml");
        }
    }
}
