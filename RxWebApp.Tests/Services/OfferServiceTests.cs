using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RxWebApp.Data;
using RxWebApp.Data.Entities;
using RxWebApp.Services;

namespace RxWebApp.Tests.Services
{
    [TestClass]
    public class OfferServiceTests
    {
        [TestMethod]
        public void GetOffersForOrder_ForwardsRequestToUnderlyingRepository()
        {
            // GIVEN
            Mock<IOfferRepository> repo = new Mock<IOfferRepository>();
            repo.Setup(x => x.GetOffersForOrder(It.IsAny<int>())).Returns((int id) => Observable.Return(new List<Offer> { new Offer(new OfferEntity { Id = id }) }));
            IOfferService service = new OfferService(repo.Object);

            // WHEN
            List<Offer> offers = null;
            service.GetOffersForOrder(42)
                .Subscribe(x => offers = x.ToList());

            // THEN
            Assert.IsNotNull(offers);
            Assert.AreEqual(1, offers.Count);
            Assert.AreEqual(42, offers[0].Id);
            repo.Verify(x => x.GetOffersForOrder(42), Times.Once());
        }
    }
}
