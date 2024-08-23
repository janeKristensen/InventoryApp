using InventoryManagement;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ExtensionMethods;

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

        private void EditCancel_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }

        private void EditSave_Click(object sender, RoutedEventArgs e)
        {
            inventory.EditEntry(substance, Convert.ToInt32(txt_Stock.Text), txt_Name.Text, txt_Type.Text, txt_Unit.Text);
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }

        private void EditDelete_Click(object sender, RoutedEventArgs e)
        {
            inventory.RemoveSubstance(substance);
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }

        private void Txt_Stock_Input(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(txt_Stock.Text))
            {
                MessageBox.Show("Stock must be a numeric value");
                txt_Stock.Background = Brushes.Red;
            }   
        }
    }
}
