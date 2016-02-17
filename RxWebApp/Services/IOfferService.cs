using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using RxWebApp.Data;

namespace RxWebApp.Services
{
    internal interface IOfferService
    {
        IObservable<IEnumerable<Offer>> GetOffersForOrder(int orderId);

        IObservable<IEnumerable<Offer>> GetOffersForOrder(int orderId, IScheduler scheduler);
    }
}