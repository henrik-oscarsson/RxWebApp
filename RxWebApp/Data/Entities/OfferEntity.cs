namespace RxWebApp.Data.Entities
{
    internal class OfferEntity : Entity
    {
        public int ProductId { get; set; }

        public string ProductDescription { get; set; }

        public decimal Price { get; set; }
    }
}
