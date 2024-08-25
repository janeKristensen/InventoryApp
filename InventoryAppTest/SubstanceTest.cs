using InventoryManagement;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InventoryAppTest
{
       
    [TestClass]
    public class SubstanceTest 
    {
        Substance _substance = new("Betamethasone", "B02", "10 mg/vial", 200, "RS");

        [TestInitialize]
        public void Initialize()
        {
            Assert.IsNotNull(_substance);
        }

        [TestMethod]
        public void Test_SubstractStock_Substance_stock_equals_150()
        {  
            _substance.SubtractStock(50);
            Assert.AreEqual(150, _substance.Stock);
        }

        public void Test_AddStock_Substance_stock_equals_200()
        {
            _substance.AddStock(50);
            Assert.AreEqual(200, _substance.Stock);
        }

        [TestMethod]
        public void Test_Stock_Amount_less_than_1_throws_exception()
        {
            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => new Substance("Betamethasone", "B02", "10 mg/vial", 0, "RS"));
        }

        [TestMethod]
        public void Test_Edit_Substance()
        {
                _substance.EditSubstance(100, "TestBetamethasone", "IS", "TestUnit");
            Assert.AreEqual(100, _substance.Stock);
        }
    }

    [TestClass]
    public class InventoryTest
    {
        Inventory _inventory = Inventory.GetInstance();
        [TestInitialize]
        public void Initialize()
        {
            Assert.IsNotNull(_inventory);

            var stock = _inventory.GetStock();
            if (stock != null)
            {
                foreach (var substance in stock)
                {
                    _inventory.RemoveSubstance(substance);
                }
            }

            _inventory.NewSubstance("Delgocitinib", "A01", "10 mg/vial", 200, "RS");
            _inventory.NewSubstance("Calcipotriol", "D45", "200 mg/vial", 100, "RS");
        }

        [TestMethod]
        public void Test_FindSubstance_RemoveSubstance_find_and_remove_substances()
        {
            var stock = _inventory.GetStock();
            Assert.IsNotNull(stock);

            var found_sub = _inventory.FindSubstance(stock[0].Id);
            Assert.AreEqual(stock[0].Id, found_sub.Id);

            foreach (var substance in stock)
            {
                _inventory.RemoveSubstance(substance);
            }
        }

        [TestMethod]
        public void Test_EditEntry_substance_return_updated_values()
        {
            var stock = _inventory.GetStock();
            Assert.IsNotNull(stock);

            int amount = 1000;
            string name = "TestName";
            string unit = "100 mg/vial";
            string type = "newType";
            _inventory.EditEntry(stock[0], amount, name, type, unit);

            var stock_edited = _inventory.GetStock();
            Assert.IsNotNull(stock_edited);

            Assert.AreEqual(stock[0].Name, stock_edited[0].Name);
            Assert.AreEqual(stock[0].Stock, stock_edited[0].Stock);
            Assert.AreEqual(stock[0].Unit, stock_edited[0].Unit);
            Assert.AreEqual(stock[0].RefType, stock_edited[0].RefType);
        }

        [TestMethod]
        public void Test_Update_stock_of_substance_updated()
        {
            var stock = _inventory.GetStock();
            Assert.IsNotNull(stock);

            var order_details = new List<OrderItemsData>
            {
                new OrderItemsData(stock[0].Id, stock[0].Name, stock[0].BatchNumber, stock[0].Unit, 10),
                new OrderItemsData(stock[1].Id, stock[1].Name, stock[1].BatchNumber, stock[1].Unit, 50)
            };

            _inventory.Update(new Order("Test", "Test"), order_details);

            var stock_edited = _inventory.GetStock();
            Assert.IsNotNull(stock_edited);
            Assert.AreEqual(stock_edited[0].Stock, 190);
            Assert.AreEqual(stock_edited[1].Stock, 50);
        }
    }

    [TestClass]
    public class OrderManagementTest
    {
        OrderManagement _orderManagement = OrderManagement.GetInstance();
        Inventory _inventory = Inventory.GetInstance();

        [TestInitialize] 
        public void TestInitialize() 
        {
            Assert.IsNotNull(_inventory);
            Assert.IsNotNull(_orderManagement); 
            _orderManagement.Attach(_inventory);

            _inventory.NewSubstance("Delgocitinib", "A01", "10 mg/vial", 200, "RS");
            _inventory.NewSubstance("Calcipotriol", "D45", "200 mg/vial", 500, "RS");

            var stock = _inventory.GetStock();
            Assert.IsNotNull(stock);

            var order_details = new List<OrderItemsData>
            {
                new OrderItemsData(stock[0].Id, stock[0].Name, stock[0].BatchNumber, stock[0].Unit, 10),
                new OrderItemsData(stock[1].Id, stock[1].Name, stock[1].BatchNumber, stock[1].Unit, 50)
            };

            _orderManagement.AddOrder("TestReceiver", "TestAddress", order_details);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var clear_stock = _inventory.GetStock();
            if (clear_stock != null)
            {
                foreach (var substance in clear_stock)
                {
                    _inventory.RemoveSubstance(substance);
                }
            }

            var clear_orders = _orderManagement.GetAllOrders();
            if (clear_orders != null)
            {
                foreach (var order in clear_orders)
                {
                    _orderManagement.DeleteOrder(order);
                }
            }
        }

        [TestMethod]
        public void Test_AddOrder_add_new_order_and_update_substance_stock()
        {
            Assert.IsNotNull(_orderManagement.GetAllOrders());

            var stock_edited = _inventory.GetStock();
            Assert.IsNotNull(stock_edited);
            Assert.AreEqual(190, stock_edited[0].Stock);
            Assert.AreEqual(450, stock_edited[1].Stock);
        }

        [TestMethod]
        public void Test_UpdateOrder_address_and_reciver_updated()
        {
            var orders = _orderManagement.GetAllOrders();
            Assert.IsNotNull(orders);
            orders[0].Receiver = "NewTestReceiver";
            orders[0].Address = "NewTestAddress";
            _orderManagement.UpdateOrder(orders[0]);
            
            var db_order = _orderManagement.GetAllOrders().First();
            Assert.AreEqual(orders[0].Receiver, db_order.Receiver);
            Assert.AreEqual(orders[0].Address, db_order.Address);
        }

        [TestMethod]
        public void Test_UpdateOrderDetail_update_amount_on_order_and_on_substance_stock()
        {
            var orders = _orderManagement.GetAllOrders();
            Assert.AreNotEqual(0, orders.Count);

            var stock = _inventory.GetStock();
            var order_detail = new OrderItemsData(stock[1].Id, stock[1].Name, stock[1].BatchNumber, stock[1].Unit, 40);
            _orderManagement.UpdateOrderDetail(order_detail);

            var stock_edited = _inventory.GetStock();
            Assert.AreEqual(460, stock_edited[1].Stock);
        }

        [TestMethod]
        public void Test_GetOrderData_Assert_equal_true()
        {
            var orders = _orderManagement.GetAllOrders();
            Assert.AreNotEqual(0, orders.Count);   

            var order_data = _orderManagement.GetOrderData(orders[0]);
            Assert.AreEqual("Delgocitinib", order_data[0].Name);
            Assert.AreEqual("A01", order_data[0].Batch);
            Assert.AreEqual("10 mg/vial", order_data[0].Unit);
            Assert.AreEqual(10, order_data[0].Amount);

        }

        [TestMethod]
        public void Test_DeleteOrder_DeleteDetail_Assert_order_and_detail_is_null_true()
        {
            var orders = _orderManagement.GetAllOrders();
            Assert.AreEqual(orders.Count, 1);
            _orderManagement.DeleteOrder(orders[0]);

            var orders_deleted = _orderManagement.GetAllOrders();
            Assert.AreEqual(0, orders_deleted.Count);

            var details_deleted = _orderManagement.GetOrderDetails(orders[0]);
            Assert.AreEqual(0, details_deleted.Count);
        }
    }

   

}