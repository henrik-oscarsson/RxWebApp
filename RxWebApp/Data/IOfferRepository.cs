using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using RxWebApp.Data.Entities;

namespace RxWebApp.Data
{
    internal interface IOfferRepository : IEntityRepository<OfferEntity>
    {
        IObservable<IEnumerable<Offer>> GetOffersForOrder(int orderId);

        IObservable<IEnumerable<Offer>> GetOffersForOrder(int orderId, IScheduler scheduler);
    }
}