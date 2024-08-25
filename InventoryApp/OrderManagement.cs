
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace InventoryManagement
{
    internal interface IEventManager
    {
        void Attach(IListener listener);
        void Detach(IListener listener);
        void Notify(Order order, List<OrderItemsData> orderdetails);
    }


    internal sealed class OrderManagement : IEventManager
    {
        private readonly List<IListener> _listeners = new List<IListener>();
        private Inventory inventory = Inventory.GetInstance();

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

        public void Notify(Order order, List<OrderItemsData> orderdetails)
        {
            foreach (var listener in _listeners)
            {
                listener.Update(order, orderdetails);
            }
        }

        public void AddOrder(string receiver, string address, List<OrderItemsData> orderdetails)
        {
            using (var db = new SubstanceContext())
            {
                var order = new Order(receiver, address);
                var db_order = db.Add(order);
                db.SaveChanges();

                foreach (var item in orderdetails)
                {
                    db.Add(new OrderDetail(db_order.Entity.Id, item.Id, item.Amount));
                    db.SaveChanges();
                }
               
                Notify(order, orderdetails);
            }; 
        }

        public void UpdateOrder(Order order)
        { 
            using (var db = new SubstanceContext())
            {
                var db_order = db.Orders.Find(order.Id);
                db.Entry(db_order).CurrentValues.SetValues(order);
                db.SaveChanges();
            }
        }

        public void UpdateOrderDetail(OrderItemsData item)
        {
            using (var db = new SubstanceContext())
            {
                var detail = db.OrderDetails.Where(x => x.SubstanceId == item.Id).First();
                var substance = db.ReferenceSubstances.Find(detail.SubstanceId);
                if (item.Amount > detail.Amount)
                {
                    substance.SubtractStock((item.Amount - detail.Amount));
                }
                else 
                {
                    substance.AddStock((detail.Amount - item.Amount));
                }
                
                db.Entry(detail).CurrentValues.SetValues(item);
                db.SaveChanges();
            }
        }

        public void DeleteOrder(Order order)
        {
            using (var db = new SubstanceContext())
            {
                var db_order = db.Orders.Find(order.Id);
                var details = db.OrderDetails.Where(x => x.OrderId == order.Id).ToList();
                if(details != null)
                {
                    foreach (var item in details)
                    {
                        db.OrderDetails.Remove(item);
                    }
                    db.SaveChanges();
                }

                db.Entry(db_order).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void DeleteDetail(OrderItemsData item)
        {
            using (var db = new SubstanceContext())
            {
                db.Remove(db.OrderDetails.Single(x => x.DetailId == item.Id));
                db.SaveChanges();
            }
        }

        public List<OrderItemsData> GetOrderData(Order order)
        {
            List<OrderItemsData> orderData = new List<OrderItemsData>();
            using (var db = new SubstanceContext())
            {
                var _orderDetails = GetOrderDetails(order);
                foreach (var item in _orderDetails)
                {
                    var substance = inventory.FindSubstance(item.SubstanceId);
                    orderData.Add(new OrderItemsData(item.DetailId, substance.Name, substance.BatchNumber, substance.Unit, item.Amount));
                }
            };

            return orderData;
        }

        public List<Order> GetAllOrders()
        {
            using (var db = new SubstanceContext())
            {
                return db.Orders.ToList();
            }
        }

        public Order FindOrder(int id)
        {
            using (var db = new SubstanceContext())
            {
                return db.Orders.Find(id);
            }
        }

        public List<OrderDetail> GetOrderDetails(Order order) 
        {
            using (var db = new SubstanceContext())
            {
                return db.OrderDetails.Where(x => x.OrderId == order.Id).ToList();
               
            };
        }

        public void PrintOrder(Order order)
        {
            using (var db = new SubstanceContext())
            {
                var data = GetOrderDetails(order);
                Console.WriteLine($"{order.Receiver}\n{order.Address}\nSubstance order:");
                foreach (var line in data)
                {
                    var substance = inventory.FindSubstance(line.SubstanceId);
                    Console.WriteLine($"{substance.ToString()}: {line.Amount}");
                }
            }
        }

        public void PrintAllOrders()
        {
            var items = GetAllOrders();
            foreach (var item in items)
            {
                PrintOrder(item);
            }
        }
    }
}
