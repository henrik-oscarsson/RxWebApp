using System.Web;
using RxWebApp.Data;

namespace RxWebApp
{
    public class WebDataContextFactory : IDataContextFactory
    {
        public DataContext Current
        {
            get
            {
                return (DataContext)HttpContext.Current.Items["_DataContext"];
            }

            set
            {
                HttpContext.Current.Items["_DataContext"] = value;
            }
        }
    }
}