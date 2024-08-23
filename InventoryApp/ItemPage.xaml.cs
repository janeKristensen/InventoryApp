using InventoryManagement;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for ItemPage.xaml
    /// </summary>
    internal partial class ItemPage : Page
    {
        readonly Substance substance;
        // Singleton instance of inventory and Ordermanagement
        readonly Inventory inventory = Inventory.GetInstance();


        public ItemPage()
        {
            InitializeComponent(); 
        }

        public ItemPage(Substance substance) : this()
        {
            this.substance = substance;
            this.Loaded += new RoutedEventHandler(ItemPage_Loaded); 
        }

        void ItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            OnInit();
        }

        private void OnInit()
        {
            txt_Name.Text = substance.Name;
            txt_Batch.Text = substance.BatchNumber;
            txt_Unit.Text = substance.Unit;
            txt_Type.Text = substance.RefType;
            txt_Stock.Text = substance.Stock.ToString();
        }

        public void BackToMain()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new Uri("SubstanceOverviewPage.xaml", UriKind.Relative));
        }

        private void EditCancel_Click(object sender, RoutedEventArgs e)
        {
            BackToMain();
        }

        private void EditSave_Click(object sender, RoutedEventArgs e)
        {
            inventory.EditEntry(substance, Convert.ToInt32(txt_Stock.Text), txt_Name.Text, txt_Batch.Text, txt_Type.Text, txt_Unit.Text);
            BackToMain();
        }

        private void EditDelete_Click(object sender, RoutedEventArgs e)
        {
            inventory.RemoveSubstance(substance);
            BackToMain();
        }

        private void Txt_Stock_Input(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text))
            {
                MessageBox.Show("Stock must be a numeric value");
                TextBox source = e.Source as TextBox;
                source.Text = "";
            }   
        }
    }
}
