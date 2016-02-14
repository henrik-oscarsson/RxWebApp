using RxWebApp.Data.Entities;

namespace RxWebApp.Data
{
    public class Offer
    {
        private readonly OfferEntity _backingField;

        internal Offer(OfferEntity entity)
        {
            _backingField = entity;
        }

        public int Id => _backingField.Id;

        public string ProductDescription => _backingField.ProductDescription;
    }
}