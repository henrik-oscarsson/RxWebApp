namespace RxWebApp.Data.Entities
{
    internal class OrderEntity : Entity
    {
        public decimal Price { get; set; }

        public int CustomerId { get; set; }
    }
}
