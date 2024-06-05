
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InventoryManagement
{
    public class OrderDetail
    {
        public OrderDetail (int OrderId, string SubstanceId, int Amount) 
        {
            this.OrderId = OrderId;
            this.SubstanceId = SubstanceId;
            this.Amount = Amount;
   

            /*
            using (var db = new SubstanceContext())
            {
                this.Substance = db.ReferenceSubstances.Where(e => e.BatchNumber == SubstanceId).First();
                this.Order = db.Orders.Where(e => e.Id == OrderId).First();
            }*/
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get;  }
        public int Amount { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("SubstanceId")]
        public virtual Substance Substance { get; set; }
        public string SubstanceId { get; set; }
    }

    public class Order
    {
        public Order(string address) 
        { 
            Address = address;
        }

        [Key]
        public int Id { get; set; }

        public string Address { get; set; }

        [ForeignKey("DetailId")]
        public ICollection<OrderDetail> OrderDetails { get; set; }

        
        public void PrintOrder()
        {
            Console.WriteLine($"{Address}\nSubstance order:");

            if (OrderDetails != null )
            {
                foreach (var line in OrderDetails)
                {
                    Console.WriteLine($"{line.Substance.Name} {line.Substance.Unit}: {line.Substance.Stock} pcs.");
                }
            }
            else { Console.WriteLine("0 items in order.."); }
        }
    }
}
