using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using RxWebApp.Data;

namespace RxWebApp.Services
{
    internal interface IOrderService
    {
        IObservable<IEnumerable<Order>> GetAllOrders(IScheduler scheduler);
        IObservable<IEnumerable<Order>> GetAllOrders();

        IObservable<Order> CreateOrder(int customerId, IScheduler scheduler);
        IObservable<Order> CreateOrder(int customerId);

        IObservable<Unit> DeleteOrder(int orderId, IScheduler scheduler);
        IObservable<Unit> DeleteOrder(int orderId);
    }
}