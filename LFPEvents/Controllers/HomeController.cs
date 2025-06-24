using System.Web.Mvc;

namespace LFPEvents.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "About";
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Contact";
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Title = "Privacy";
            return View();
        }
    }
}
