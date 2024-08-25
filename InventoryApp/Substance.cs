using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace InventoryManagement
{
    internal class Substance
    {
        private int _stock;

        public Substance(string Name, string BatchNumber, string Unit, int Stock, string RefType)
        {
            this.Name = Name;
            this.BatchNumber = BatchNumber;
            this.Unit = Unit;
            this.Stock = Stock;
            this.RefType = RefType;
        }

        // Constructor used for adding substances to order
        public Substance(string Name, string BatchNumber, string Unit, int Stock)
        {
            this.Name = Name;
            this.BatchNumber = BatchNumber;
            this.Unit = Unit;
            this.Stock = Stock;
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
       
        public string BatchNumber { get; set; }

        public List<OrderDetail> OrderDetails { get; private set; }

        public string Unit { get; set; }

        public int Stock 
        { 
            get { return _stock; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Stock must be more than 0!", nameof(_stock));
                }
                _stock = value;
            }
        }

        public string RefType { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name}, {BatchNumber}, {Unit}";
        }

        public void EditSubstance(int amount, string name, string type, string unit)
        {
            Stock = amount;
            Name = name;
            RefType = type;
            Unit = unit;
        }

        public void AddStock(int amount)
        {
            this.Stock += amount;
        }

        public void SubtractStock(int amount)
        {
            this.Stock -= amount;
        }
    }
}



