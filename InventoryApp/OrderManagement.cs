
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace InventoryManagement
{
    internal interface IEventManager
    {
        void Attach(IListener listener);
        void Detach(IListener listener);
        void Notify(Order order, List<(int, int)> orderdetails);
    }


    internal sealed class OrderManagement : IEventManager
    {
        private readonly List<IListener> _listeners = new List<IListener>();

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

        public void Notify(Order order, List<(int, int)> orderdetails)
        {
            foreach (var listener in _listeners)
            {
                listener.Update(order, orderdetails);
            }
        }

  
        
        // Create a new order and return
        public void AddOrder(string receiver, string address, List<(int, int)> orderItems)
        {
            using (var db = new SubstanceContext())
            {
                var order = new Order(receiver, address);
                var db_order = db.Add(order);
                db.SaveChanges();

                foreach (var item in orderItems)
                {
                    var substance = db.ReferenceSubstances.Where(e => e.Id == item.Item1).First();
                    var detail = new OrderDetail(db_order.Entity.Id, substance.Id, item.Item2);
                    db.Add(detail);
                    db.SaveChanges();
                }
               
                this.Notify(order, orderItems);
            }; 
        }


        public void PrintOrders()
        {
            using (var db = new SubstanceContext())
            {
                var items = db.Orders.ToList();
                foreach (var item in items)
                {
                    item.PrintOrder();
                }
            }
        }
    }
}
