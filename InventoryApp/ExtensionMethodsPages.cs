using InventoryApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ExtensionMethods
{
    public static class ExtensionMethodsPages
    {
        public static void NavigateTo(string page)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new Uri(page, UriKind.Relative));
        }
    }
}
