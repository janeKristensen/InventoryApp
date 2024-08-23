using ExtensionMethods;
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
    /// Interaction logic for NewSubstancePage.xaml
    /// </summary>
    internal partial class NewSubstancePage : Page
    {
        // Singleton instance of inventory and Ordermanagement
        private readonly Inventory inventory = Inventory.GetInstance();

        public NewSubstancePage()
        {
            InitializeComponent();
        }

        private void BtnAddSubstance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inventory.NewSubstance(
                    txtBox_SubstanceName.Text, 
                    txtBox_BatchNumber.Text, 
                    txtBox_Unit.Text, 
                    Convert.ToInt32(txtBox_Amount.Text), 
                    txtBox_Type.Text);

                ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occured while adding the substance. Try again!\n{exception}");
            }           
        }

        private void BtnCancelNew_Click(object sender, RoutedEventArgs e)
        {
            ExtensionMethodsPages.NavigateTo("SubstanceOverviewPage.xaml");
        }

        private void Txt_Amount_Input(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(txtBox_Amount.Text))
            {
                MessageBox.Show("Stock must be a numeric value");
                txtBox_Amount.Background = Brushes.Red;
            }
        }
    }
}
