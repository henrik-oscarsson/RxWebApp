using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using RxWebApp.Data;

namespace RxWebApp.Services
{
    internal interface IOrderService
    {
        IObservable<IEnumerable<Order>> GetAllOrders(IScheduler scheduler = null);
        IObservable<Order> CreateOrder(int customerId, IScheduler scheduler = null);
        IObservable<Unit> DeleteOrder(Order order, IScheduler scheduler = null);
    }
}