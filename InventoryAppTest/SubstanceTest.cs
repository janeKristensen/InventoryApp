using InventoryManagement;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InventoryAppTest
{
       
    [TestClass]
    public class SubstanceTest 
    { 

        [TestMethod]
        public void Test_Stock()
        {
            var substance = new InventoryManagement.Substance("Betamethasone", "B02", "10 mg/vial", 200, "RS");
            Assert.IsNotNull(substance);
            substance.SubtractStock(50);
            Assert.AreEqual(150, substance.Stock);

            substance.AddStock(50);
            Assert.AreEqual(200, substance.Stock);
        }

        [TestMethod]
        public void Test_Stock_Exception()
        {
            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => new InventoryManagement.Substance("Betamethasone", "B02", "10 mg/vial", -10, "RS"));
        }

        [TestMethod]
        public void Test_Edit_Substance()
        {
            Substance sub = new Substance("Betamethasone", "B02", "10 mg / vial", 200, "RS");

            try
            {
                sub.EditSubstance(100, "TestBetamethasone", "IS", "TestUnit");
            }catch(FileNotFoundException)
            {
                Console.WriteLine("Not connected to database");
            }

            Assert.AreEqual(100, sub.Stock);
        }
    }

    [TestClass]
    public class OrderTest
    {
        [TestMethod]
        public void Test_Print_Order() 
        {
            var order = new Order("test", "text_address");

        }
    }
}