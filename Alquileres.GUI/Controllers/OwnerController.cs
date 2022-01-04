using Alquileres.Acces.Utilities;
using Alquileres.GUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alquileres.GUI.Controllers
{
    public class OwnerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public OwnerController(ILogger<HomeController> logger, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            OwnerVM ownerVM = new OwnerVM();
            ownerVM.Response = null;

           
            return View("Index", ownerVM);
        }

        public IActionResult AddOwner()
        {
            string fileName = string.Concat(Guid.NewGuid().ToString(), Path.GetExtension(this.HttpContext.Request.Form.Files[0].FileName));

            try
            {
                Utilities utilities = new Utilities();
                utilities.SaveFile(this.HttpContext, string.Concat(_environment.ContentRootPath, "\\", "Files", "\\", "Owner", "\\", fileName));

                string json = string.Format("   {{\"name\":\"{0}\"," +
                                "\"address\":\"{1}\"" +
                                ",\"photo\":\"{2}\"" +
                                ",\"birthDay\":\"{3}\"}}",
                                Convert.ToString(this.HttpContext.Request.Form["OwnerName"]),
                                Convert.ToString(this.HttpContext.Request.Form["OwnerAdress"]),
                                fileName,
                                Convert.ToString(this.HttpContext.Request.Form["OwnerBirthday"]));
                var strResponse = utilities.SendRequestHttp(_configuration.GetSection("ApiUrls:Owners:AddOwner").Value, json);

                while (!strResponse.IsCompleted)
                    strResponse.Wait();

                OwnerVM ownerVM = new OwnerVM();
                ownerVM.Response = strResponse.Result;
                return View("Index", ownerVM);
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(string.Concat(_environment.ContentRootPath, "\\", "Files", "\\", "Owner", "\\", fileName)))
                    System.IO.File.Delete(string.Concat(_environment.ContentRootPath, "\\", "Files", "\\", "Owner", "\\", fileName));
                _logger.LogError(ex.Message);

                OwnerVM ownerVM = new OwnerVM();
                ownerVM.Response = "An error has ocurred please try later.";
                return View("Index", ownerVM);
            }
        }
    }
}
