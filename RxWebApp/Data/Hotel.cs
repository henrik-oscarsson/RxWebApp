using RxWebApp.Data.Entities;

namespace RxWebApp.Data
{
    public class Hotel
    {
        private readonly HotelEntity _backingField;

        internal Hotel(HotelEntity entity)
        {
            _backingField = entity;
        }

        public int Id => _backingField.Id;

        public string Name => _backingField.Name;

        public string City => _backingField.City;

        public string Description => _backingField.Description;
    }
}