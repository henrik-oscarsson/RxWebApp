using System.Web.Mvc;

namespace RxWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Orders()
        {
            ViewBag.Message = "Your orders.";
            return new RedirectResult("Orders");
        }
    }
}