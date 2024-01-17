using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class BlazeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
