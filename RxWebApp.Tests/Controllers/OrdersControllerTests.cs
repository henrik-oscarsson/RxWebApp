using System;
using System.Reactive.Concurrency;
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
            // GIVEN
            IoC.Instance.Register<IOrderRepository, OrderRepository>();
            IoC.Instance.Register<ISchedulerService>(() => new SchedulerService());

            // Create a mock service
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            IoC.Instance.RegisterInstance(mockOrderService.Object);

            // Configure it to return a specific object when its CreateOrder() method is called.
            mockOrderService.Setup(x => x.CreateOrder(It.IsAny<int>(), It.IsAny<IScheduler>())).Returns((int id) => Observable.Return(new Order(new OrderEntity { Id = id })));
            // Create the controller that we want to test.
            OrdersController controller = new OrdersController();

            // WHEN

            // Call the CreateOrder method, and see what happens.
            ActionResult result = await controller.CreateOrder(17);

            // THEN

            // the controller should call the IOrderService.CreateOrder() method, once.
            mockOrderService.Verify(x => x.CreateOrder(17, It.IsAny<IScheduler>()), Times.Once());

            // the result returned is valid
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateOrder_HandlesTimeoutGracefully()
        {
            // GIVEN
            IoC.Instance.Register<IOrderRepository, OrderRepository>();
            IoC.Instance.Register<ISchedulerService>(() => new SchedulerService());
            var scheduler = IoC.Instance.Resolve<ISchedulerService>();

            // Create a mock service
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            IoC.Instance.RegisterInstance(mockOrderService.Object);

            // Configure it to return a specific object when its CreateOrder() method is called.
            mockOrderService.Setup(x => x.CreateOrder(It.IsAny<int>(), It.IsAny<IScheduler>())).Returns((int id) =>
            {
                // Simulate a 90 second delay
                return Observable
                    .Return(new Order(new OrderEntity {Id = id}))
                    .Delay(TimeSpan.FromSeconds(90), scheduler.Pool); // Simulate a long running request
            });

            // Create the controller that we want to test.
            OrdersController controller = new OrdersController();

            // WHEN

            // Call the CreateOrder method, and see what happens.
            ActionResult result = await controller.CreateOrder(17);

            // THEN

            // the controller should call the IOrderService.CreateOrder() method, once.
            mockOrderService.Verify(x => x.CreateOrder(17, It.IsAny<IScheduler>()), Times.Once());

            // but the result (due to timeout) must be a "not found".
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
    }
}
