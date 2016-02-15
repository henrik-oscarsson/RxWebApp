using System.Collections.Generic;
using RxWebApp.Data;

namespace RxWebApp.ViewModels
{
    public class OrdersViewModel
    {
        public OrdersViewModel(int customerId, IEnumerable<Order> orders)
            : this(customerId, orders, null)
        {
        }

        public OrdersViewModel(int customerId, IEnumerable<Offer> offers)
            : this(customerId, null, offers)
        {
        }

        private OrdersViewModel(int customerId, IEnumerable<Order> orders, IEnumerable<Offer> offers)
        {
            CustomerId = customerId;
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

        public int CustomerId { get; private set; }

        public IList<Order> AllOrders { get; private set; }

        public IList<Offer> AllOffers { get; private set; }
    }
}