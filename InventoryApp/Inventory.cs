
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
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

        public void EditEntry(Substance s, int amount, string name, string batch, string type, string unit)
        {
            using (var db = new SubstanceContext())
            {
                var sub = db.ReferenceSubstances.Find(s.BatchNumber);
                if (sub != null && s.BatchNumber == batch)
                {
                    sub.Name = name;
                    sub.Unit = unit;
                    sub.RefType = type;
                    sub.Stock = amount;
                    db.SaveChanges();
                }
                else
                {
                    /*
                    var answer = MessageBox.Show("Changing batch number is not allowed. Would you like to create a new substance?", "Edit was not saved!", MessageBoxButton.YesNo);
                    if (answer == MessageBoxResult.Yes)
                    {
                        // Add to database and save
                        db.Add(new Substance(name, batch, unit, Convert.ToInt32(amount), type));
                        db.SaveChanges();
                        return sub;
                    }*/

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
        public void PressButtonForNewSubstance(string name, string batchnumber, string unit, string amount, string type)
        {
            using (var db = new SubstanceContext())
            {
                // Add to database and save
                db.Add(new Substance(name, batchnumber, unit, Convert.ToInt32(amount), type));
                db.SaveChanges();
            };
        }


        public List<Substance> GetStock()
        {
            using (var db = new SubstanceContext())
            {
                var items = db.ReferenceSubstances.ToList();
                return items;
            }        
        }
    }
}
