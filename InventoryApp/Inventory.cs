
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Xml.Linq;

namespace InventoryManagement
{
    public interface IListener
    {
        void Update(Order order);
    }


    public sealed class Inventory : IListener
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
        public void Update(Order order)
        {
            //Console.WriteLine("\nOrder received in inventory!\n");
            
            using (var db = new SubstanceContext())
            {
                foreach (var line in order.OrderDetails)
                {
                    var sub = db.ReferenceSubstances.Find(line.Substance.BatchNumber);
                    if (sub != null)
                    {
                        sub.Stock -= line.Amount;
                        db.SaveChanges();
                    }
                }
            };  
        }


        public void RemoveSubstance(Substance substance)
        {
            using (var db = new SubstanceContext())
            {
                db.ReferenceSubstances.Attach(substance);
                db.ReferenceSubstances.Remove(substance);
                db.SaveChanges();
            };
        }


        // To be implemented with GUI
        // Take input from user and add to database
        public void PressButtonForNewSubstance(string name, string batchnumber, string unit, string amount)
        {
            using (var db = new SubstanceContext())
            {
                // Add to database and save
                string type = "RS";
                db.Add(new Substance(name, batchnumber, unit, Convert.ToInt32(amount), type));
                db.SaveChanges();
            };
        }


        public void PrintStock()
        {
            using (var db = new SubstanceContext())
            {
                var items = db.ReferenceSubstances.ToList();
                foreach (var item in items)
                {
                    //Console.WriteLine($"{item.Name}, {item.BatchNumber}, {item.Unit}: {item.Stock}");
                }
            }        
        }
    }
}
