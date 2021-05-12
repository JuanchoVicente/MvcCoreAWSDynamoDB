using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSDynamoDB.Helpers;
using MvcCoreAWSDynamoDB.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSDynamoDB.Controllers
{
    public class AWSFilesController : Controller
    {
        private UploadHelper uploadhelper;
        public ServiceAWSS3 service;

        public AWSFilesController(UploadHelper upload
            , ServiceAWSS3 service)
        {
            this.uploadhelper = upload;
            this.service = service;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListFilesAWS()
        {
            
            List<String> files = await this.service.GetS3FilesAsync();
            return View(files);            
        }

        public IActionResult UploadFileAWS()
        {
            return View();
        }
 
        [HttpPost]
        public async Task<IActionResult> UploadFileAWS(IFormFile file)
        {
            String path =
                await this.uploadhelper.UploadFileAsync(file, Folders.Images);

            using (FileStream stream = new FileStream(path
                , FileMode.Open, FileAccess.Read))
            {
                bool respuesta =
                    await this.service.UploadFileAsync(stream, file.FileName);
                ViewData["MENSAJE"] = "Archivo en AWS Bucket: " + respuesta;
            };
            return View();
        }

        public async Task<IActionResult> FileAWS(String fileName)
        {
            Stream stream =
            await this.service.GetFileAsync(fileName);
            return File(stream, "image/png");
        }

        public async Task<IActionResult> DeleteFileAWS(String fileName)
        {
            await this.service.DeleteFileAsync(fileName);
            return RedirectToAction("Index");
        }

        public IActionResult PermisosBucket()
        {
            return View();
        }
    }
}
