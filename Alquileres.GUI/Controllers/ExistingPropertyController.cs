using Alquileres.Acces.Utilities;
using Alquileres.GUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alquileres.GUI.Controllers
{
    public class ExistingPropertyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public ExistingPropertyController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            PropertyVM propertyVM = new PropertyVM();
            try
            {
                Utilities utilities = new Utilities();

                var responseProperty = await utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Property:GetBasicProperty").Value);
                var dataProperty = utilities.DeserializeObjectJson(responseProperty, typeof(PropertyEntity[])) as PropertyEntity[];

                var responseOwners = await utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Owners:GetOwners").Value);
                var dataOwners = utilities.DeserializeObjectJson(responseOwners, typeof(OwnerEntity[])) as OwnerEntity[];

                foreach (var property in dataProperty)
                {
                    property.OwnerName = dataOwners.Where(x => x.IdOwner == property.idOwner).FirstOrDefault().Name;
                }
                propertyVM.Property = dataProperty;
                propertyVM.Response = null;

                return View("~/Views/Property/ExistingProperty.cshtml", propertyVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                propertyVM.Response = ex.Message;
                propertyVM.Property = new PropertyEntity[] { };

                return View("~/Views/Property/ExistingProperty.cshtml", propertyVM);
            }

        }
    }
}
