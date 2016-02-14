using Microsoft.Owin;
using Owin;
using RxWebApp.Data;
using RxWebApp.Services;

[assembly: OwinStartupAttribute(typeof(RxWebApp.Startup))]
namespace RxWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            IoC.Instance.Register<IDataContextFactory, WebDataContextFactory>();
            IoC.Instance.Register<IOrderRepository, OrderRepository>();
            IoC.Instance.Register<IOfferRepository, OfferRepository>();
            IoC.Instance.Register<IOrderService, OrderService>();
            IoC.Instance.Register<IOfferService, OfferService>();
        }
    }
}
