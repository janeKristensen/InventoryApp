using InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    internal partial class OrderPage : Page
    {
        private Order _order;
        private List<OrderDetail> _orderDetails = new List<OrderDetail>();
        private List<OrderItemsData> _orderData = new List<OrderItemsData>();

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

            using (var db = new SubstanceContext())
            {
                _orderDetails = db.OrderDetails.Where(x => x.OrderId == _order.Id).ToList();
                foreach (var item in _orderDetails)
                {
                    var substance = db.ReferenceSubstances.Where(x => x.Id == item.SubstanceId).First();
                    _orderData.Add(new OrderItemsData(item.DetailId, substance.Name, substance.BatchNumber, substance.Unit ,item.Amount.ToString()));
                } 
            };

            OrderDetailView.ItemsSource = _orderData;
        }

        private void PrintOrder(object sender, RoutedEventArgs e)
        {
            _order.PrintOrder();
        }

        private void BackToMain()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new Uri("ViewOrderPage.xaml", UriKind.Relative));
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            using (var db = new SubstanceContext())
            {
                var data = db.Orders.Where(x => x.Id == _order.Id).First();

                data.Receiver = txt_Receiver.Text;  
                data.Address = txt_Address.Text;

                db.SaveChanges();
            }

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
            OrderItemPage p = new OrderItemPage(item, _order);
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.NavigationService.Navigate(p);
        }

        private void CancelOrder(object sender, RoutedEventArgs e)
        {
            BackToMain();
        }
    }
}
