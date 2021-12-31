using Alquileres.Acces.Utilities;
using Alquileres.GUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alquileres.GUI.Controllers
{
    public class ExistingOwner : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        public ExistingOwner(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }
        public IActionResult Index()
        {
            try
            {
                ExistingOwnerVM existingOwnerVM = new ExistingOwnerVM();
                existingOwnerVM.Response = null;


                Utilities utilities = new Utilities();
                var response = utilities.GetResponseHttp("https://localhost:44361/api/Owner/GetOwners");
                while (!response.IsCompleted) { };
                if (response.Status == TaskStatus.Faulted)
                    throw new Exception(response.Exception.Message);

                var data = utilities.DeserializeObjectJson(response.Result, typeof(OwnerEntity[])) as OwnerEntity[];
                foreach (var x in data)
                {
                    x.Photo = string.Concat(@"/Owners/", x.Photo);
                }

                existingOwnerVM.Data = data;

                return View("Index", existingOwnerVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                ExistingOwnerVM ownerVM = new ExistingOwnerVM();
                ownerVM.Response = "An error has ocurred please try later.";
                ownerVM.Data = null;

                return View("Index", ownerVM);
            }
        }
    }
}
