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

        public IObservable<IEnumerable<Order>> GetAllOrders()
        {
            return Observable.Return(_allOrders.Values);
        }

        public IObservable<IEnumerable<Order>> GetAllOrders(IScheduler scheduler)
        {
            return Observable.Return(_allOrders.Values, scheduler);
        }

        #region CRUD

        public IObservable<Order> CreateOrder(int customerId)
        {
            return _orderRepository
                .CreateOrder(customerId)
                .Select(dbOrder => dbOrder.ToObject())
                .Do(order => _allOrders.Add(order.Id, order));
        }

        public IObservable<Order> CreateOrder(int customerId, IScheduler scheduler)
        {
            return _orderRepository
                .CreateOrder(customerId, scheduler)
                .Select(dbOrder => dbOrder.ToObject())
                .Do(order => _allOrders.Add(order.Id, order));
        }

        public IObservable<Unit> DeleteOrder(int orderId)
        {
            return _orderRepository
                .DeleteOrder(orderId)
                .Do(_ =>
                {
                    _allOrders.Remove(orderId);
                });
        }

        public IObservable<Unit> DeleteOrder(int orderId, IScheduler scheduler)
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