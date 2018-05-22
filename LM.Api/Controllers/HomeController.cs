using System.Web.Mvc;

namespace LM.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            return View();
        }
    }
}
