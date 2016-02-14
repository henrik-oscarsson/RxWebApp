using System.Collections.Generic;
using RxWebApp.Data;

namespace RxWebApp.ViewModels
{
    public class OrdersViewModel
    {
        public OrdersViewModel(IEnumerable<Order> orders)
            : this(null, orders, null)
        {
        }

        public OrdersViewModel(Order order, IEnumerable<Offer> offers)
            : this(order, null, offers)
        {
        }

        private OrdersViewModel(Order order, IEnumerable<Order> orders, IEnumerable<Offer> offers)
        {
            Order = order;
            AllOffers = new List<Offer>();
            AllOrders = new List<Order>();

            if (orders != null)
            {
                foreach (Order o in orders)
                {
                    AllOrders.Add(o);
                }
            }

            if (offers != null)
            {
                foreach (Offer o in offers)
                {
                    AllOffers.Add(o);
                }
            }
        }

        public Order Order { get; private set; }

        public IList<Order> AllOrders { get; private set; }

        public IList<Offer> AllOffers { get; private set; }
    }
}