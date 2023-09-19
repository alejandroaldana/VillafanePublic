using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Villafane.Domain.Models.DB;
using Villafane.Domain.ViewModels;
using Villafane.Services.DB;

namespace Villafane.WebSite.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly ConfigurationBusinessManager _manager;

        public ConfigurationController(ConfigurationBusinessManager manager)
        {
            _manager = manager;
        }
        public async Task<IActionResult> Index()
        {
            var config = await _manager.ObtenerConfiguracionConLogoAsync();
            if (config != null && config.IdLogoNavigation != null)
            {
                ViewBag.Logo = config.IdLogoNavigation.BinarioLogo;
            }

            var configViewModel = new ConfigurationViewModel();
            if (config != null)
            {
                configViewModel.ColorBarra = config.ColorBarra;
                configViewModel.ColorPrincipal = config.ColorPrincipal;
                configViewModel.ColorCabeceraTabla = config.ColorCabecerasTablas;
                configViewModel.ColorCabeceraTablaVertical = config.ColorCabeceraTablasVertical;
                configViewModel.IdLogo = config.IdLogo;
                configViewModel.Id = config.Id;
            }
            return View(configViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConfigurationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Configuracion config = new Configuracion();
                config.ColorBarra = model.ColorBarra;
                config.ColorPrincipal = model.ColorPrincipal;
                config.ColorCabecerasTablas = model.ColorCabeceraTabla;
                config.ColorCabeceraTablasVertical = model.ColorCabeceraTablaVertical;
                if (model.Id != null)
                {
                    config.Id = model.Id.Value;
                }
                else
                {
                    config.Id = 0;
                }
                if (model.IdLogo != null)
                {
                    config.IdLogo = model.IdLogo.Value;
                }
                else
                {
                    config.IdLogo = null;
                }
                if (model.Logo != null && model.Logo.Length > 0)
                {
                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Logo.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                    // Obtener el Content-Type del archivo
                    string contentType = model.Logo.ContentType;
                    Console.WriteLine("Content-Type of the file: " + contentType);

                    // Obtener la extensión del archivo
                    string fileExtension = Path.GetExtension(model.Logo.FileName);
                    Console.WriteLine("Extension of the file: " + fileExtension);


                    config.IdLogoNavigation = new LogotipoCliente()
                    {
                        BinarioLogo = fileBytes,
                        ContentTypeLogo = contentType,
                        ExtensionLogo = fileExtension
                    };
                }

                await _manager.ModificarConfiguracion(config);
            }

            return View(model);
        }

        public async Task<IActionResult> Logs()
        {
            //List<string> usuarios = new List<string>()
            //{
            //    "ines.miralles@villafane.com",
            //    "teresa.honorato@villafane.com",
            //    "david.garcia@villafane.com",
            //    "alexandra.gonzalez@villafane.com",
            //    "alejandro.aldana@villafane.com",
            //    "maria.damia@villafane.com",
            //    "powerbi@villafane.com",
            //    "carlos.gomez@villafane.com",
            //    "fernando.gonzalez@villafane.com",
            //    "elena.crespo@villafane.com"
            //};
            //List<int> dias = new List<int>()
            //{
            //    1,2,3,4,5,6,7,8,9,
            //    10,11,12,13,14,15,16,17,18,19,20,21,22,
            //    23,24,25
            //};

            //List<string> pantallas = new List<string>()
            //{
            //    "GDI",
            //    "Home",
            //    "Indicadores",
            //    "Pesos Teoricos",
            //    "Valores",
            //    "Variables"
            //};

            //for (int i = 0; i < 300; i++)
            //{

            //    var randomuser = new Random();
            //    int index = randomuser.Next(usuarios.Count);

            //    var randodia = new Random();
            //    int indexdia = randodia.Next(dias.Count);
                
            //    var randopantalla = new Random();
            //    int indexpantalla= randopantalla.Next(pantallas.Count);


            //    var log = new Log()
            //    {
            //        Usuario = usuarios[index],
            //        Fecha = new DateTime(2023, 07, dias[indexdia]),
            //        Pantalla = pantallas[indexpantalla]
            //        ,
            //        Accion = "Acceso"
            //    };
            //    await _manager.GenerarLog(log);
            //}
            var logs = await _manager.ObtenerLogs();
            return View(logs);
        }

    
    }
}
