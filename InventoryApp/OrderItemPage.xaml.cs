using ExtensionMethods;
using InventoryManagement;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for OrderItemPage.xaml
    /// </summary>
    internal partial class OrderItemPage : Page
    {

        private OrderItemsData _item;
        private Order _order;
        // Singleton instance of inventory and Ordermanagement
        private readonly OrderManagement _orderManagement = OrderManagement.GetInstance();


        public OrderItemPage()
        {
            InitializeComponent();
        }

        public OrderItemPage(OrderItemsData detail, Order order) : this()
        {
            _item = detail;
            _order = order;
            Loaded += new RoutedEventHandler(OrderItemPage_Loaded);
        }

        private void OrderItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            OnInit();
        }

        private void OnInit()
        {
            txt_Name.Text = _item.Name;
            txt_Batch.Text = _item.Batch;
            txt_Unit.Text = _item.Unit;
            txt_Amount.Text = _item.Amount.ToString();
        }

        private void EditCancel_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateToPage(new OrderPage(_order));
        }

        private void EditSave_Click(object sender, RoutedEventArgs e)
        {
            _item.Amount = Convert.ToInt32(txt_Amount.Text);
            _orderManagement.UpdateOrderDetail(_order, _item);

            ExtensionMethodsPages.NavigateToPage(new OrderPage(_order));
        }

        private void EditDelete_Click(object sender, RoutedEventArgs e)
        {
            _orderManagement.DeleteDetail(_item);
            ExtensionMethodsPages.NavigateToPage(new OrderPage(_order));
        }

        private void Txt_Amount_Input(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text))
            {
                MessageBox.Show("Amunt must be a numeric value");
                TextBox source = e.Source as TextBox;
                source.Text = "";
            }
        }
    }
}

