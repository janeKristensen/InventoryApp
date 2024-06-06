using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;

namespace InventoryManagement
{
    public class Substance
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
        public Substance(string Name, string BatchNumber, string Unit,int Amount)
        {
            this.Name = Name;
            this.BatchNumber = BatchNumber;
            this.Unit = Unit;
            this.Stock = Amount;
        }

        public string Name
        {
            get; set;
        }

        [Key]
        public string BatchNumber
        {
            get; set;
        }

        public List<OrderDetail> OrderDetails { get; set; }

        public string Unit
        {
            get; set;
        }

        public int Stock
        {
            get; set;
        } = 0;

        public string RefType
        {
            get; set;
        } = string.Empty;

        public void addStock(int amount)
        {
            this.Stock += amount;
        }

        public void subtractStock(int amount)
        {
            this.Stock -= amount;
        }

        public virtual void DoTypeStuff()
        {
            Console.WriteLine("Doing substance stuff");
        }
        
    }



    public class ReferenceSubstance : Substance
    {
        public ReferenceSubstance(string Name, string BatchNumber, string VialSize, int Stock, string Type) : base(Name, BatchNumber, VialSize, Stock, Type) { }

        public override void DoTypeStuff()
        {
            Console.WriteLine("Doing Reference Substance stuff");
        }
    }



    public class InternalStandard : Substance
    {
        public InternalStandard(string Name, string BatchNumber, string VialSize, int Stock, string Type) : base(Name, BatchNumber, VialSize, Stock, Type) { }

        public override void DoTypeStuff()
        {
            Console.WriteLine("Doing Internal Standard stuff");
        }
    }
}


