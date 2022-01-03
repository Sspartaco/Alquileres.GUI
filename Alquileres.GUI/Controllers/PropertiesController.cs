using Alquileres.Acces.Utilities;
using Alquileres.GUI.Models;
using Alquileres.GUI.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Alquileres.GUI.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public PropertiesController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string idProperty)
        {
            FullPropertyVM fullPropertyVM = new FullPropertyVM();
            try
            {
                Utilities utilities = new Utilities();
                var response = await utilities.GetResponseHttp(string.Concat(_configuration.GetSection("ApiUrls:Property:GetFullProperty").Value, "?idProperty=", idProperty));
                var data = utilities.DeserializeObjectJson(response, typeof(FullPropertyVM)) as FullPropertyVM;

                fullPropertyVM.PropertyTrace = data.PropertyTrace;
                fullPropertyVM.PropertyEntity = data.PropertyEntity;
                fullPropertyVM.PropertyImage = data.PropertyImage;

                fullPropertyVM.Response = null;

                return View("~/Views/Property/Properties.cshtml", fullPropertyVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                fullPropertyVM.Response = ex.Message;
                fullPropertyVM.PropertyTrace = new PropertyTraceEntity();
                fullPropertyVM.PropertyEntity = new PropertyEntity();
                fullPropertyVM.PropertyImage = new PropertyImageEntity();

                return View("~/Views/Property/Properties.cshtml", fullPropertyVM);
            }
        }
    }
}
