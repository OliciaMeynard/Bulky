using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class BlazeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
