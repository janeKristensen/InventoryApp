

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;


namespace InventoryManagement
{
    internal interface IListener
    {
        void Update(Order order, List<OrderItemsData> orderdetails);
    }


    internal sealed class Inventory : IListener
    {
        // Class is implemented as a singleton
        private static Inventory _instance;

        public static Inventory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Inventory();
            }
            return _instance;
        }


        // Implementation of interface method update
        public void Update(Order order, List<OrderItemsData> orderdetails)
        {
            using (var db = new SubstanceContext())
            {
                foreach (var line in orderdetails)
                {
                    var sub = db.ReferenceSubstances.Find(line.Id);
                    if (sub != null)
                    {
                        sub.SubtractStock(line.Amount);
                        db.SaveChanges();
                    }
                }
            };  
        }

        public void EditEntry(Substance substance, int amount, string name, string type, string unit)
        {
            using (var db = new SubstanceContext())
            {
                var db_sub = db.ReferenceSubstances.Find(substance.Id);
                substance.EditSubstance(amount, name, type, unit);
                db.Entry(db_sub).CurrentValues.SetValues(substance);
                db.SaveChanges();

            };
        }


        public void RemoveSubstance(Substance substance)
        {
            using (var db = new SubstanceContext())
            {
                var details = db.OrderDetails.Where(x => x.SubstanceId == substance.Id).ToList();
                foreach (var item in details)
                {
                    var order = db.Orders.Find(item.OrderId);
                    db.Entry(order).State = EntityState.Deleted;
                    db.OrderDetails.Remove(item);
                }
                db.Entry(substance).State = EntityState.Deleted;
                db.SaveChanges();
            };
        }

        public void NewSubstance(string name, string batchnumber, string unit, int amount, string type)
        {
            using (var db = new SubstanceContext())
            {
                db.Add(new Substance(name, batchnumber, unit, amount, type));
                db.SaveChanges();
            };
        }

        public Substance FindSubstance(int id)
        {
            using (var db = new SubstanceContext())
            {
                return db.ReferenceSubstances.Find(id);
            };
        }

        public List<Substance> GetStock()
        {
            using (var db = new SubstanceContext())
            {
                return db.ReferenceSubstances.ToList();
            }        
        }
    }
}
