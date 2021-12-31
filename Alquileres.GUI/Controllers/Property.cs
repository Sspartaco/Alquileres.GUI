using Microsoft.AspNetCore.Mvc;

namespace Alquileres.GUI.Controllers
{
    public class Property : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
