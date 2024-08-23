using InventoryManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InventoryManagement
{
    internal class OrderDetail
    {
        private int _amount;

        public OrderDetail(int OrderId, int SubstanceId, int Amount)
        {
            this.OrderId = OrderId;
            this.SubstanceId = SubstanceId;
            this.Amount = Amount;
        }

        [Key]
        public int DetailId { get; }
        public int Amount 
        { 
            get { return _amount; }
            set {
                if (value <= 0) 
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Amount must be above 0!");
                }
                _amount = value; 
            } 
        }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("SubstanceId")]
        public virtual Substance Substance { get; set; }
        public int SubstanceId { get; set; }

        public void SaveChanges()
        {
            using (var db = new SubstanceContext())
            {
                var detail = db.OrderDetails.Find(this.DetailId);
                if (detail != null)
                {
                    db.Entry(detail).CurrentValues.SetValues(this);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Order was not found in database", "An error occurred!", MessageBoxButton.OK);
                }
            }
        }
    }

    internal class OrderItemsData
    {
        public OrderItemsData(int id, string name, string batch, string unit, int amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
            Batch = batch;
            Unit = unit;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Batch { get; set; }
        public string Unit { get; set; }

        public void SaveChanges()
        {
            using (var db = new SubstanceContext())
            {
                var detail = db.OrderDetails.Find(this.Id);
                if (detail != null)
                {
                    detail.Amount = this.Amount;
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Order was not found in database", "An error occurred!", MessageBoxButton.OK);
                }
            }
        }

        public void DeleteItem()
        {
            using (var db = new SubstanceContext())
            {
                db.Remove(db.OrderDetails.Single(x => x.DetailId == this.Id));
                db.SaveChanges();
            }
        }
    };
}
