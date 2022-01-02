using Alquileres.Acces.Utilities;
using Alquileres.GUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alquileres.GUI.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public PropertyController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            PropertyVM propertyVM = new PropertyVM();
            Utilities utilities = new Utilities();
            try
            {
                var response = utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Owners:GetOwners").Value);
                while (!response.IsCompleted) { };
                if (response.Status == TaskStatus.Faulted)
                    throw new Exception(response.Exception.Message);

                var data = utilities.DeserializeObjectJson(response.Result, typeof(OwnerEntity[])) as OwnerEntity[];

                propertyVM.Response = null;
                propertyVM.Owners = data;

                return View("Index", propertyVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                var response = utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Owners:GetOwners").Value);
                OwnerEntity[]? data = new OwnerEntity[] { };
                while (!response.IsCompleted) { };
                if (response.Status != TaskStatus.Faulted)
                {
                    data = utilities.DeserializeObjectJson(response.Result, typeof(OwnerEntity[])) as OwnerEntity[];
                    propertyVM.Owners = data;
                }
                else
                    propertyVM.Owners = new OwnerEntity[] { };

                propertyVM.Response = "An error has ocurred please try later.";


                return View("Index", propertyVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProperty()
        {
            PropertyVM propertyVM = new PropertyVM();
            Utilities utilities = new Utilities();

            try
            {
                var response = utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Owners:GetOwners").Value);
                while (!response.IsCompleted) { };
                if (response.Status == TaskStatus.Faulted)
                    throw new Exception(response.Exception.Message);

                var data = utilities.DeserializeObjectJson(response.Result, typeof(OwnerEntity[])) as OwnerEntity[];

                string json = string.Format("{{\"name\":\"{0}\"," +
                                            "\"address\":\"{1}\"" +
                                            ",\"price\":\"{2}\"" +
                                            ",\"codeInternal\":\"{3}\"" +
                                            ",\"year\":\"{4}\"" +
                                            ",\"idOwner\":\"{5}\"}}",
                                            Convert.ToString(this.HttpContext.Request.Form["PropertyName"]),
                                            Convert.ToString(this.HttpContext.Request.Form["PropertyAdress"]),
                                            Convert.ToString(this.HttpContext.Request.Form["PropertyPrice"]),
                                            Convert.ToString(this.HttpContext.Request.Form["CodeInternal"]),
                                            Convert.ToString(this.HttpContext.Request.Form["PropertyYear"]),
                                            Convert.ToString(this.HttpContext.Request.Form["PropertyOwner"][0]));
                var strResponse = await utilities.SendRequestHttp(_configuration.GetSection("ApiUrls:Property:AddProperty").Value, json);

                propertyVM.Response = strResponse.Replace("'", "");
                propertyVM.Owners = data;

                return View("Index", propertyVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                var response = utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Owners:GetOwners").Value);
                OwnerEntity[]? data = new OwnerEntity[] { };
                while (!response.IsCompleted) { };
                if (response.Status != TaskStatus.Faulted)
                {
                    data = utilities.DeserializeObjectJson(response.Result, typeof(OwnerEntity[])) as OwnerEntity[];
                    propertyVM.Owners = data;
                }
                else
                    propertyVM.Owners = new OwnerEntity[] { };

                propertyVM.Response = "An error has ocurred please try later.";

                return View("Index", propertyVM);
            }
        }
    }
}
