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
        private readonly IWebHostEnvironment _environment;

        public PropertiesController(IConfiguration configuration, ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _logger = logger;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string idProperty)
        {
            FullPropertyVM fullPropertyVM = new FullPropertyVM();
            Utilities utilities = new Utilities();

            try
            {
                var response = await utilities.GetResponseHttp(string.Concat(_configuration.GetSection("ApiUrls:Property:GetFullProperty").Value, "?idProperty=", idProperty));
                var data = utilities.DeserializeObjectJson(response, typeof(FullPropertyVM)) as FullPropertyVM;

                if (data.PropertyImage.File != null)
                    data.PropertyImage.File = string.Concat(@"/Property/", data.PropertyImage.File);
                fullPropertyVM.PropertyTrace = data.PropertyTrace;
                fullPropertyVM.Property = data.Property;
                fullPropertyVM.PropertyImage = data.PropertyImage;

                fullPropertyVM.Response = null;

                return View("~/Views/Property/Properties.cshtml", fullPropertyVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                PropertyVM propertyVM = new PropertyVM();

                var responseProperty = await utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Property:GetBasicProperty").Value);
                var dataProperty = utilities.DeserializeObjectJson(responseProperty, typeof(PropertyEntity[])) as PropertyEntity[];

                var responseOwners = await utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Owners:GetOwners").Value);
                var dataOwners = utilities.DeserializeObjectJson(responseOwners, typeof(OwnerEntity[])) as OwnerEntity[];

                foreach (var property in dataProperty)
                {
                    property.OwnerName = dataOwners.Where(x => x.IdOwner == property.idOwner).FirstOrDefault().Name;
                }

                propertyVM.Response = ex.Message;
                propertyVM.Property = dataProperty;

                return View("~/Views/Property/ExistingProperty.cshtml", propertyVM);
            }
        }

        public async Task<IActionResult> AddFullProperty()
        {
            FullPropertyVM fullPropertyVM = new FullPropertyVM();
            Utilities utilities = new Utilities();

            try
            {
                var response = await utilities.GetResponseHttp(string.Concat(_configuration.GetSection("ApiUrls:Property:GetBasicPropertyById").Value, "?idProperty=", this.HttpContext.Request.Form["HiddenIdProperty"]));
                var data = utilities.DeserializeObjectJson(response, typeof(PropertyEntity)) as PropertyEntity;
                string fileName = null;

                if (this.HttpContext.Request.Form.Files.Count > 0)
                {
                    fileName = string.Concat(Guid.NewGuid().ToString(), Path.GetExtension(this.HttpContext.Request.Form.Files[0].FileName));
                    utilities.SaveFile(this.HttpContext, string.Concat(_environment.ContentRootPath, "\\", "Files", "\\", "Property", "\\", fileName));
                }

                string json = string.Format("{{\"property\":{{\"idProperty\":\"{0}\"," +
                                                        "\"name\":\"{1}\"" +
                                                        ",\"address\":\"{2}\"" +
                                                        ",\"price\":\"{3}\"" +
                                                        ",\"codeInternal\":\"{4}\"" +
                                                        ",\"year\":{5}" +
                                                        ",\"idOwner\":\"{6}\"}}," +
                                             "\"propertyImage\":{{\"idPropertyImage\":\"{7}\"" +
                                                             ",\"idProperty\":\"{8}\"" +
                                                             ",\"file\":\"{9}\"" +
                                                             ",\"enable\":{10}}}," +
                                             "\"propertyTrace\":{{\"idPropertyTrace\":\"{11}\"" +
                                                             ",\"dateSale\":\"{12}\"" +
                                                             ",\"name\":\"{13}\"" +
                                                             ",\"value\":{14}" +
                                                             ",\"tax\":{15}" +
                                                             ",\"idProperty\":\"{16}\"}}}}",
                                             data.IdProperty,
                                             data.Name,
                                             data.Address,
                                             data.Price,
                                             data.CodeInternal,
                                             data.Year,
                                             data.idOwner,
                                             Convert.ToString(this.HttpContext.Request.Form["HiddenIdPropertyImage"]),
                                             data.IdProperty,
                                             this.HttpContext.Request.Form.Files.Count > 0 ? fileName : this.HttpContext.Request.Form["HiddenPropertyImageName"].ToString(),
                                             true,
                                             Convert.ToString(this.HttpContext.Request.Form["HiddenIdPropertyTrace"]),
                                             Convert.ToString(Convert.ToDateTime(this.HttpContext.Request.Form["DateSale"]).ToString("yyyy-MM-ddThh:mm")),
                                             Convert.ToString(this.HttpContext.Request.Form["NameTrace"]),
                                             Convert.ToDecimal(this.HttpContext.Request.Form["ValueTrace"]),
                                             Convert.ToDecimal(this.HttpContext.Request.Form["TaxTrace"]),
                                             data.IdProperty);

                json = json.Replace("True", "true");
                var strResponse = await utilities.SendRequestHttp(_configuration.GetSection("ApiUrls:Property:UpdateFullProperty").Value, json);

                var responseFullProperty = await utilities.GetResponseHttp(string.Concat(_configuration.GetSection("ApiUrls:Property:GetFullProperty").Value, "?idProperty=", data.IdProperty));
                var dataFullProperty = utilities.DeserializeObjectJson(responseFullProperty, typeof(FullPropertyVM)) as FullPropertyVM;

                dataFullProperty.PropertyImage.File = string.Concat(@"/Property/", dataFullProperty.PropertyImage.File);
                fullPropertyVM.PropertyTrace = dataFullProperty.PropertyTrace;
                fullPropertyVM.Property = dataFullProperty.Property;
                fullPropertyVM.PropertyImage = dataFullProperty.PropertyImage;
                fullPropertyVM.Response = strResponse;

                return View("~/Views/Property/Properties.cshtml", fullPropertyVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                PropertyVM propertyVM = new PropertyVM();

                var responseProperty = await utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Property:GetBasicProperty").Value);
                var dataProperty = utilities.DeserializeObjectJson(responseProperty, typeof(PropertyEntity[])) as PropertyEntity[];

                var responseOwners = await utilities.GetResponseHttp(_configuration.GetSection("ApiUrls:Owners:GetOwners").Value);
                var dataOwners = utilities.DeserializeObjectJson(responseOwners, typeof(OwnerEntity[])) as OwnerEntity[];

                foreach (var property in dataProperty)
                {
                    property.OwnerName = dataOwners.Where(x => x.IdOwner == property.idOwner).FirstOrDefault().Name;
                }

                propertyVM.Response = ex.Message;
                propertyVM.Property = dataProperty;

                return View("~/Views/Property/ExistingProperty.cshtml", propertyVM);
            }
        }
    }
}
