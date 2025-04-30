using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
