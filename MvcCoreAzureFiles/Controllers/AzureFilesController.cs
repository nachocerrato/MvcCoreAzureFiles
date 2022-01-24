using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureFiles.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAzureFiles.Controllers
{
    public class AzureFilesController : Controller
    {
        private ServiceStorageFile service;

        public AzureFilesController(ServiceStorageFile service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index(string filename)
        {
            if(filename != null)
            {
                //Leemos el contenido del fichero
                string data =
                    await this.service.ReadFileAsync(filename);
                ViewData["CONTENIDO"] = data;
            }

            List<string> files = await this.service.GetFilesAsync();
            return View(files);
        }

        public async Task<IActionResult> DeleteFiles(string filename)
        {
            await this.service.DeleteFileAsync(filename);
            return RedirectToAction("Index");
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            string filename = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadFileAsync(filename, stream);
            }
            ViewData["MENSAJE"] = "Fichero subido a Azure";
            return View();
        }

    }
}
