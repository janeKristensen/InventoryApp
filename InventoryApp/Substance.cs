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

        public int Stock { get; set; }

        public string RefType { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name}, {BatchNumber}, {Unit}";
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



