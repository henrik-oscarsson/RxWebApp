using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using RxWebApp.Data;

namespace RxWebApp.Services
{
    internal sealed class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;

        public OfferService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public IObservable<IEnumerable<Offer>> GetOffersForOrder(int orderId, IScheduler scheduler = null)
        {
            return _offerRepository.GetOffersForOrder(orderId, scheduler);
        }
    }
}