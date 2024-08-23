using InventoryManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InventoryApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App 
    {
        public App()
        {
            // Singleton instance of inventory and Ordermanagement
            Inventory inventory = Inventory.GetInstance();
            OrderManagement orderManagement = OrderManagement.GetInstance();
            // Inventory will subscribe to Ordermanagement to receive notifications on new orders
            orderManagement.Attach(inventory);
        }
    }
}
