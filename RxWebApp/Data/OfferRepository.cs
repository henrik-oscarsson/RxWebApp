using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using RxWebApp.Data.Entities;

namespace RxWebApp.Data
{
    internal sealed class OfferRepository : RepositoryBase, IOfferRepository
    {
        public OfferRepository(IDataContextFactory dbFactory)
            : base(dbFactory)
        {
        }

        public IObservable<IEnumerable<Offer>> GetOffersForOrder(int orderId)
        {
            return GetOffersForOrder(orderId, null);
        }

        public IObservable<IEnumerable<Offer>> GetOffersForOrder(int orderId, IScheduler scheduler)
        {
            if (scheduler != null)
            {
                return Observable.Return(new List<Offer>
                {
                    new Offer(new OfferEntity {Id = 1, ProductDescription = "foo"}),
                    new Offer(new OfferEntity {Id = 2, ProductDescription = "bar"}),
                    new Offer(new OfferEntity {Id = 3, ProductDescription = "baz"}),
                }, scheduler);
            }
            return Observable.Return(new List<Offer>
                {
                    new Offer(new OfferEntity {Id = 1, ProductDescription = "foo"}),
                    new Offer(new OfferEntity {Id = 2, ProductDescription = "bar"}),
                    new Offer(new OfferEntity {Id = 3, ProductDescription = "baz"}),
                });
        }
    }
}