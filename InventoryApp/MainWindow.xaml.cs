using InventoryManagement;
using Microsoft.IdentityModel.Tokens;
using System.Windows;


namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Singleton instance of inventory and Ordermanagement
        Inventory inventory = Inventory.GetInstance();
        OrderManagement orderManagement = OrderManagement.GetInstance();

        public MainWindow()
        {
           

        }

        private void BtnAddSubstance_Click(object sender, RoutedEventArgs e)
        {
            var name = txtBox_SubstanceName.Text;
            var batch = txtBox_BatchNumber.Text;
            var unit = txtBox_Unit.Text;
            var amount = txtBox_Amount.Text;
            
            if (!string.IsNullOrEmpty(name) &&
                !string.IsNullOrEmpty(batch) &&
                !string.IsNullOrEmpty(unit) &&
                !string.IsNullOrEmpty(amount))
            {
                try { 
                    inventory.PressButtonForNewSubstance(name, batch, unit, amount);
                }
                catch
                {
                    //pop up with error

                    txtBox_SubstanceName.Clear();
                    txtBox_BatchNumber.Clear();
                    txtBox_Unit.Clear();
                    txtBox_Amount.Clear();
                }
                    
            }
            
        }
    }
}
