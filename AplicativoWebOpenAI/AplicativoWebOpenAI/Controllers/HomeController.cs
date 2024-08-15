using Microsoft.AspNetCore.Mvc;

namespace AplicativoWebOpenAI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
