using InventoryManagement;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
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
    /// Interaction logic for ItemPage.xaml
    /// </summary>
    public partial class ItemPage : Page
    {
        MainWindow main = Application.Current.MainWindow as MainWindow;
        Substance sub;
        // Singleton instance of inventory and Ordermanagement
        Inventory inventory = Inventory.GetInstance();

        public ItemPage()
        {
            InitializeComponent();
            sub = main.DatabaseView.SelectedItem as Substance;
            OnInit();
        }

        private void OnInit()
        {
            txt_Name.Text = sub.Name;
            txt_Batch.Text = sub.BatchNumber;
            txt_Unit.Text = sub.Unit;
            txt_Type.Text = sub.RefType;
            txt_Stock.Text = sub.Stock.ToString();
        }

        public void BackToMain()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Visibility = Visibility.Visible;
            Window win = (Window)this.Parent;
            win.Close();
            mainWindow.OnInit();
        }

        private void EditCancel_Click(object sender, RoutedEventArgs e)
        {
            BackToMain();
        }

        private void EditSave_Click(object sender, RoutedEventArgs e)
        {
            inventory.EditEntry(sub, Convert.ToInt32(txt_Stock.Text), txt_Name.Text, txt_Batch.Text, txt_Type.Text, txt_Unit.Text);
            BackToMain();
        }

        private void EditDelete_Click(object sender, RoutedEventArgs e)
        {
            inventory.RemoveSubstance(sub);
            BackToMain();
        }
    }
}
