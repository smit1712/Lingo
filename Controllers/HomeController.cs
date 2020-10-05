using Microsoft.AspNetCore.Mvc;

namespace Lingo_WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
