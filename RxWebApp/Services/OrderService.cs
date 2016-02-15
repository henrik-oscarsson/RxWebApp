using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using RxWebApp.Data;
using RxWebApp.Extensions;

namespace RxWebApp.Services
{
    internal sealed class OrderService : IOrderService
    {
        private readonly Dictionary<int, Order> _allOrders;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _allOrders = new Dictionary<int, Order>();
            _orderRepository = orderRepository;

            // Populate with dummy orders
            Observable.Range(0, 10)
                .SelectMany(i => _orderRepository.CreateOrder(i))
                .Select(dbOrder => dbOrder.ToObject())
                .Subscribe(order =>
                {
                    _allOrders.Add(order.Id, order);
                });
        }

        public IObservable<IEnumerable<Order>> GetAllOrders(IScheduler scheduler = null)
        {
            return (scheduler != null) ?
                Observable.Return(_allOrders.Values, scheduler) :
                Observable.Return(_allOrders.Values);
        }

        #region CRUD

        public IObservable<Order> CreateOrder(int customerId, IScheduler scheduler = null)
        {
            return _orderRepository
                .CreateOrder(customerId, scheduler)
                .Select(dbOrder => dbOrder.ToObject())
                .Do(order => _allOrders.Add(order.Id, order));
        }

        public IObservable<Unit> DeleteOrder(int orderId, IScheduler scheduler = null)
        {
            return _orderRepository
                .DeleteOrder(orderId, scheduler)
                .Do(_ =>
                {
                    _allOrders.Remove(orderId);
                });
        }

        #endregion
    }
}