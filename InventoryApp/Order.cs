
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InventoryManagement
{
    internal class Order
    {
        public Order(string receiver, string address)
        {
            Receiver = receiver;
            Address = address;
        }

        [Key]
        public int Id { get; set; }
        public string Receiver { get; set; }
        public string Address { get; set; }
        public List<OrderDetail> OrderDetails { get; private set; }

        
    }

}