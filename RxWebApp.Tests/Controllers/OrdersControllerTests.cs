using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RxWebApp.Controllers;
using RxWebApp.Data;
using RxWebApp.Data.Entities;
using RxWebApp.Services;

namespace RxWebApp.Tests.Controllers
{
    [TestClass]
    public class OrdersControllerTests
    {
        [TestMethod]
        public async Task CreateOrder_PerformsServerRequest()
        {
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            IoC.Instance.RegisterInstance(mockOrderRepository.Object);
            Mock<IOfferRepository> mockOfferRepository = new Mock<IOfferRepository>();
            IoC.Instance.RegisterInstance(mockOfferRepository.Object);
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            IoC.Instance.RegisterInstance(mockOrderService.Object);
            Mock<IOfferService> mockOfferService = new Mock<IOfferService>();
            IoC.Instance.RegisterInstance(mockOfferService.Object);
            IoC.Instance.Register<ISchedulerService>(() => new SchedulerService());

            // Configure it to return a specific object when its CreateOrder() method is called.
            mockOrderService.Setup(x => x.CreateOrder(It.IsAny<int>())).Returns((int id) => Observable.Return(new Order(new OrderEntity { Id = id })));

            // GIVEN
            OrdersController controller = new OrdersController();

            // WHEN

            // Call the CreateOrder method, and see what happens.
            ActionResult result = await controller.Create();

            // THEN

            // the controller should call the IOrderService.CreateOrder() method, once.
            mockOrderService.Verify(x => x.CreateOrder(42), Times.Once());

            // the result returned is valid
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateOrder_HandlesTimeoutGracefully()
        {
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            IoC.Instance.RegisterInstance(mockOrderRepository.Object);
            Mock<IOfferRepository> mockOfferRepository = new Mock<IOfferRepository>();
            IoC.Instance.RegisterInstance(mockOfferRepository.Object);
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            IoC.Instance.RegisterInstance(mockOrderService.Object);
            Mock<IOfferService> mockOfferService = new Mock<IOfferService>();
            IoC.Instance.RegisterInstance(mockOfferService.Object);
            IoC.Instance.Register<ISchedulerService>(() => new SchedulerService());
            var scheduler = IoC.Instance.Resolve<ISchedulerService>();

            // Configure it to return a specific object when its CreateOrder() method is called.
            mockOrderService.Setup(x => x.CreateOrder(It.IsAny<int>())).Returns((int id) =>
            {
                // Simulate a 90 second delay
                return Observable
                    .Return(new Order(new OrderEntity {Id = id}))
                    .Delay(TimeSpan.FromSeconds(90), scheduler.Pool); // Simulate a long running request
            });

            // GIVEN
            OrdersController controller = new OrdersController();

            // WHEN

            // Call the CreateOrder method, and see what happens.
            ActionResult result = await controller.Create();

            // THEN

            // the controller should call the IOrderService.CreateOrder() method, once.
            mockOrderService.Verify(x => x.CreateOrder(42), Times.Once());

            // but the result (due to timeout) must be a "not found".
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
    }
}
