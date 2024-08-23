using InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for OrderItemPage.xaml
    /// </summary>
    internal partial class OrderItemPage : Page
    {

        private OrderItemsData _item;
        private OrderDetail _orderDetail;
        private Order _order;
        // Singleton instance of inventory and Ordermanagement
        private readonly Inventory inventory = Inventory.GetInstance();


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
            txt_Amount.Text = _item.Amount;
        }

        public void BackToOrder()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            OrderPage p = new OrderPage(_order);
            mainWindow.MainFrame.Navigate(p);
        }

        private void EditCancel_Click(object sender, RoutedEventArgs e)
        {
            BackToOrder();
        }

        private void EditSave_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new SubstanceContext())
            {
                var record = db.OrderDetails.Where(x => x.DetailId == _item.Id).First();
                record.Amount = Convert.ToInt32(txt_Amount.Text);
                db.SaveChanges();
            }

            BackToOrder();
        }

        private void EditDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new SubstanceContext())
            {
                db.Remove(db.OrderDetails.Single(x => x.DetailId == _item.Id));
                db.SaveChanges();
            }
                BackToOrder();
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

