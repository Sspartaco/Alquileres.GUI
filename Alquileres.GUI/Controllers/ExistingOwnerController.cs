using Alquileres.Acces.Utilities;
using Alquileres.GUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alquileres.GUI.Controllers
{
    public class ExistingOwnerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public ExistingOwnerController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                ExistingOwnerVM existingOwnerVM = new ExistingOwnerVM();
                existingOwnerVM.Response = null;


                Utilities utilities = new Utilities();
                var response = utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Owners:GetOwners").Value);
                while (!response.IsCompleted) { };
                if (response.Status == TaskStatus.Faulted)
                    throw new Exception(response.Exception.Message);

                var data = utilities.DeserializeObjectJson(response.Result, typeof(OwnerEntity[])) as OwnerEntity[];
                foreach (var x in data)
                {
                    x.Photo = string.Concat(@"/Owner/", x.Photo);
                }

                existingOwnerVM.Data = data;

                return View("~/Views/Owner/ExistingOwner.cshtml", existingOwnerVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                ExistingOwnerVM ownerVM = new ExistingOwnerVM();
                ownerVM.Response = "An error has ocurred please try later.";
                ownerVM.Data = null;

                return View("~/Views/Owner/ExistingOwner.cshtml", ownerVM);
            }
        }
    }
}
