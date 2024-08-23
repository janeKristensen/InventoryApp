
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
    internal interface IListener
    {
        void Update(Order order, List<(int, int)> orderdetails);
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
        public void Update(Order order, List<(int, int)> orderdetails)
        {
            //Console.WriteLine("\nOrder received in inventory!\n");
            
            using (var db = new SubstanceContext())
            {
                foreach (var line in orderdetails)
                {
                    var sub = db.ReferenceSubstances.Where(e => e.Id == line.Item1).First();
                    if (sub != null)
                    {
                        sub.Stock -= line.Item2;
                        db.SaveChanges();
                    }
                }
            };  
        }

        public void EditEntry(Substance s, int amount, string name, string batch, string type, string unit)
        {
            using (var db = new SubstanceContext())
            {
                var sub = db.ReferenceSubstances.Find(s.Id);

                    sub.Name = name;
                    sub.Unit = unit;
                    sub.RefType = type;
                    sub.Stock = amount;
                    db.SaveChanges();

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
        public void NewSubstance(string name, string batchnumber, string unit, int amount, string type)
        {
            using (var db = new SubstanceContext())
            {
                // Add to database and save
                Substance substance = new Substance(name, batchnumber, unit, amount, type);
                var sub = db.Add(substance);
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
