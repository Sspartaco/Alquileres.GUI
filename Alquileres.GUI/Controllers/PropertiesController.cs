using Microsoft.AspNetCore.Mvc;

namespace Alquileres.GUI.Controllers
{
    public class PropertiesController : Controller
    {
        public IActionResult Index(string idProperty)
        {
            return View("~/Views/Property/Properties.cshtml");
        }
    }
}
