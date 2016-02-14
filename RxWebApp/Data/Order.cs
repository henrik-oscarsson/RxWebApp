using System.Collections.Generic;
using RxWebApp.Data.Entities;

namespace RxWebApp.Data
{
    public sealed class Order
    {
        private readonly OrderEntity _backingField;
        private readonly List<OrderRow> _rows;

        internal Order(OrderEntity entity)
        {
            _backingField = entity;
            _rows = new List<OrderRow>();
        }

        public int Id => _backingField.Id;

        public decimal TotalSum { get { return 42m; } }

        public IEnumerable<OrderRow> OrderRows => _rows;

        #region CRUD

        public OrderRow CreateRow(int productId, int quantity = 1)
        {
                var row = new OrderRow(this, productId);
                row.Quantity = quantity;
                _rows.Add(row);
                return row;
        }

        public void DeleteRow(OrderRow row)
        {
            if (row != null && row.ParentOrder == this)
            {
                _rows.Remove(row);
            }
        }

        #endregion
    }
}