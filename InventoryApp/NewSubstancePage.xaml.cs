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

namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for NewSubstancePage.xaml
    /// </summary>
    public partial class NewSubstancePage : Page
    {
        // Singleton instance of inventory and Ordermanagement
        Inventory inventory = Inventory.GetInstance();

        public NewSubstancePage()
        {
            InitializeComponent();
        }

        public void BackToMain()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Visibility = Visibility.Visible;
            Window win = (Window)this.Parent;
            win.Close();
            mainWindow.OnInit();
        }

        private void BtnAddSubstance_Click(object sender, RoutedEventArgs e)
        {
            var name = txtBox_SubstanceName.Text;
            var batch = txtBox_BatchNumber.Text;
            var unit = txtBox_Unit.Text;
            var amount = txtBox_Amount.Text;
            var type = txtBox_Type.Text;

            if (!string.IsNullOrEmpty(name) &&
                !string.IsNullOrEmpty(batch) &&
                !string.IsNullOrEmpty(unit) &&
                !string.IsNullOrEmpty(amount))
            {
                try
                {
                    inventory.PressButtonForNewSubstance(name, batch, unit, amount, type);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"An error occured while adding the substance. Try again!\n{exception}");

                    txtBox_SubstanceName.Clear();
                    txtBox_BatchNumber.Clear();
                    txtBox_Unit.Clear();
                    txtBox_Type.Clear();
                    txtBox_Amount.Clear();
                }
            }

            BackToMain();
        }

        private void BtnCancelNew_Click(object sender, RoutedEventArgs e)
        {
            BackToMain();
        }
    }
}
