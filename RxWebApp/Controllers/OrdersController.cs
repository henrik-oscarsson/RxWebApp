using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using RxWebApp.Extensions;
using RxWebApp.Services;
using RxWebApp.ViewModels;

namespace RxWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOfferService _offerService;
         
        public OrdersController()
        {
            _orderService = IoC.Instance.Resolve<IOrderService>();
            _offerService = IoC.Instance.Resolve<IOfferService>();
        }

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            return await _orderService
                .GetAllOrders()
                .Select(orders => new OrdersViewModel(orders))
                .ToActionResult(View);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(int customerId)
        {
            if (ModelState.IsValid)
            {
                var customTimeout = TimeSpan.FromSeconds(10);

                return await _orderService
                    .CreateOrder(customerId)
                    .SelectMany(order =>
                    {
                        return _offerService
                            .GetOffersForOrder(order.Id)
                            .Select(offers => new OrdersViewModel(order, offers));
                    })
                    .ToActionResult(viewModel => RedirectToAction("EditQuantity", viewModel), customTimeout);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditQuantity(int id)
        {
            if (ModelState.IsValid)
            {
            }
            return RedirectToAction("Index");
        }
    }
}