using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RxWebApp.Data;
using RxWebApp.Data.Entities;

namespace RxWebApp.Tests.Models
{
    [TestClass]
    public class OrderModelTests
    {
        [TestMethod]
        public void CreateRow_ReturnsARow_ThatIsAddedToTheOrder()
        {
            // GIVEN
            Order order = new Order(new OrderEntity { Id = 42 });
            // WHEN
            OrderRow row = order.CreateRow(1);
            // THEN
            Assert.IsNotNull(row);
            Assert.IsTrue(order.OrderRows.Any(x => x.ProductId == 1));
            Assert.AreEqual(1, order.OrderRows.Count());
        }
    }
}
