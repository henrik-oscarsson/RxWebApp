using System;
using System.Reactive;
using System.Reactive.Concurrency;
using RxWebApp.Data.Entities;

namespace RxWebApp.Data
{
    internal interface IOrderRepository : IEntityRepository<OrderEntity>
    {
        IObservable<OrderEntity> CreateOrder(int customerId, IScheduler scheduler = null);
        IObservable<Unit> DeleteOrder(int orderId, IScheduler scheduler = null);
    }
}