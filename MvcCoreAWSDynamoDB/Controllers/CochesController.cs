using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSDynamoDB.Models;
using MvcCoreAWSDynamoDB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSDynamoDB.Controllers
{
    public class CochesController : Controller
    {

        ServiceAWSDynamoDb service;

        public CochesController(ServiceAWSDynamoDb service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListCoches()
        {
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
            , int caballos, int cilindrada)
        {
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
    }
}
