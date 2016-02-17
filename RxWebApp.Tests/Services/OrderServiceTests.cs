using System;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RxWebApp.Data;
using RxWebApp.Data.Entities;
using RxWebApp.Services;

namespace RxWebApp.Tests.Services
{
    [TestClass]
    public class OrderServiceTests
    {
        [TestMethod]
        public void CreateOrder_ForwardsRequestToUnderlyingRepository()
        {
            // GIVEN
            Mock<IOrderRepository> repo = new Mock<IOrderRepository>();
            repo.Setup(x => x.CreateOrder(It.IsAny<int>())).Returns((int id) => Observable.Return(new OrderEntity { Id = id }));
            IOrderService service = new OrderService(repo.Object);

            // WHEN
            Order order = null;
            service.CreateOrder(42)
                .Subscribe(x => order = x);

            // THEN
            Assert.IsNotNull(order);
            Assert.AreEqual(42, order.Id);
            repo.Verify(x => x.CreateOrder(42), Times.Once());
        }
    }
}
