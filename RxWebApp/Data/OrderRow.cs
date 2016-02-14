namespace RxWebApp.Data
{
    public sealed class OrderRow
    {
        internal OrderRow(Order parentOrder, int productId)
        {
            ParentOrder = parentOrder;
            ProductId = productId;
        }

        public int ProductId { get; private set; }

        internal Order ParentOrder { get; private set; }

        public int Quantity { get; set; }
    }
}