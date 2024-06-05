
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace InventoryManagement
{
    public interface IEventManager
    {
        void Attach(IListener listener);
        void Detach(IListener listener);
        void Notify(Order order);
    }


    public sealed class OrderManagement : IEventManager
    {
        private List<IListener> _listeners = null;

        // Class is implemented as a singleton
        private static OrderManagement _orderManager;

        public static OrderManagement GetInstance() {
            if (_orderManager == null)
            {
                _orderManager = new OrderManagement();
            }
            return _orderManager;
        }

        // Implementation of interface methods
        public void Attach(IListener listener)
        {
            _listeners.Add(listener);
        }

        public void Detach(IListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Notify(Order order)
        {
            foreach (var listener in _listeners)
            {
                listener.Update(order);
            }
        }

        public void PrintOrders()
        {
            using (var db = new SubstanceContext())
            {
                var items = db.Orders.ToList();
                foreach (var item in items)
                {
                   //Console.WriteLine($"{item.Address}");
                }
            }
        }

        // To be implemented with GUI
        // Create a new order and return
        public void PressButtonToAddOrder()
        {
            using (var db = new SubstanceContext())
            {
                var order = new Order("Test address");
                db.Add(order);
                db.SaveChanges();

                var substance = db.ReferenceSubstances.Where(e => e.Name == "Delgocitinib").First();
                var detail = new OrderDetail(order.Id, substance.BatchNumber, 50);
                db.Add(detail);
                db.SaveChanges();

                order.OrderDetails.Add(detail);

                // Add to database and save
                
                db.SaveChanges();

                
                this.Notify(order);
            }; 
        }
    }
}
