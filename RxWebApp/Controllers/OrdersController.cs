using System.Collections.Generic;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using RxWebApp.Data;
using RxWebApp.Extensions;
using RxWebApp.Services;
using RxWebApp.ViewModels;

namespace RxWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOfferService _offerService;
        private readonly Dictionary<int, Order> _allOrders;
        private readonly int _customerId = 42;

        public OrdersController()
        {
            _orderService = IoC.Instance.Resolve<IOrderService>();
            _offerService = IoC.Instance.Resolve<IOfferService>();
            _allOrders = new Dictionary<int, Order>();
        }

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            return await _orderService
                .GetAllOrders()
                .Do(orders =>
                {
                    _allOrders.Clear();
                    foreach (Order order in orders)
                    {
                        _allOrders.Add(order.Id, order);
                    }
                })
                .Select(orders => new OrdersViewModel(_customerId, orders))
                .ToActionResult(View);
            
                // Here's an alternative way of calling 'ToActionResult'. Just to illustrate.
                //.ToActionResult(viewModel => RedirectToAction("EditQuantity", viewModel), System.TimeSpan.FromSeconds(10));
        }

        public async Task<ActionResult> Create()
        {
            if (ModelState.IsValid)
            {
                return await _orderService
                    .CreateOrder(_customerId, null)
                    .Do(order => _allOrders.Add(order.Id, order))
                    .Select(_ => new OrdersViewModel(_customerId, _allOrders.Values))
                    .ToActionResult(viewModel => RedirectToAction("Index"));
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return await _orderService
                    .DeleteOrder(id)
                    .Do(_ => _allOrders.Remove(id))
                    .Select(_ => new OrdersViewModel(_customerId, _allOrders.Values))
                    .ToActionResult(viewModel => RedirectToAction("Index"));
            }
            return RedirectToAction("Index");
        }
    }
}