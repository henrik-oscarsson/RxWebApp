using RxWebApp.Data;
using RxWebApp.Data.Entities;

namespace RxWebApp.Extensions
{
    internal static class EntityExtensions
    {
        public static Order ToObject(this OrderEntity entity)
        {
            // TODO: use ValueInjecter to do the custom conversion and possible flattening here.
            return new Order(entity);
        }
    }
}