using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using RxWebApp.Data.Entities;

namespace RxWebApp.Data
{
    internal sealed class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(IDataContextFactory dbFactory)
            : base(dbFactory)
        {
        }

        public IObservable<OrderEntity> CreateOrder(int customerId, IScheduler scheduler = null)
        {
            return (scheduler != null) ? Observable.Return(new OrderEntity { Id = customerId }, scheduler) : Observable.Return(new OrderEntity { Id = customerId });
        }

        public IObservable<Unit> DeleteOrder(int orderId, IScheduler scheduler = null)
        {
            return (scheduler != null) ? Observable.Return(new Unit(), scheduler) : Observable.Return(new Unit());
        }
    }
}