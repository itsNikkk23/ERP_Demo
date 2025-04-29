using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
