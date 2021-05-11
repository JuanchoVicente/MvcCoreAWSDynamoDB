using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSDynamoDB.Helpers;
using MvcCoreAWSDynamoDB.Models;
using MvcCoreAWSDynamoDB.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSDynamoDB.Controllers
{
    public class CochesController : Controller
    {

        ServiceAWSDynamoDb service;
        ServiceAWSS3 serviceAWSS3;

        private UploadHelper uploadHelper;

        public CochesController(ServiceAWSDynamoDb service
            , ServiceAWSS3 s3, UploadHelper uploadHelper)
        {
            this.service = service;
            this.serviceAWSS3 = s3;
            this.uploadHelper = uploadHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListCoches()
        {
            List<String> files = await this.serviceAWSS3.GetS3FilesAsync();            
            ViewData["bucket"] = files as List<String>;
            return View(await this.service.GetCochesAsync());
        }

        public async Task<IActionResult> DetailsCoche(int id)
        {
            return View(await this.service.GetCocheAsync(id));
        }

        public IActionResult CreateCar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(Coche car
            , String incluirmotor, String tipo
            , int caballos, int cilindrada, IFormFile file)
        {
            if (file != null)
            {
                String path = await this.uploadHelper.UploadFileAsync
                    (file, Folders.Images);
                using (FileStream stream = new FileStream
                    (path, FileMode.Open, FileAccess.Read))
                {
                    bool respuesta = await this.serviceAWSS3.UploadFileAsync
                        (stream, file.FileName);
                    ViewData["MENSAJE"] = "Archivo en AWS Bucket: " + respuesta;
                }
            }

                if (incluirmotor != null)
                {
                    car.Motor = new Motor();
                    car.Motor.Tipo = tipo;
                    car.Motor.Caballos = caballos;
                    car.Motor.Cilindrada = cilindrada;
                }

            await this.service.CrearCoche(car);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCar(int idCoche)
        {
            await this.service.DeleteCocheAsync(idCoche);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCar()
        {
            return View();
        }
    }
}
