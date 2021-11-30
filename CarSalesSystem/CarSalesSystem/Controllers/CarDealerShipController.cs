using System.Security.Claims;
using AutoMapper;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.CarDealership;
using CarSalesSystem.Services.CarDealerShip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class CarDealerShipController : Controller
    {
        private readonly ICarDealerShipService carDealerShipService;
        private readonly IMapper mapper;

        public CarDealerShipController(ICarDealerShipService carDealerShipService, IMapper mapper)
        {
            this.carDealerShipService = carDealerShipService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateDealership()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateDealership(CarDealershipAddFormModel dealership)
        {
            if (!ModelState.IsValid)
            {
                return View(dealership);
            }

            //TODO: Handle exceptions in front-end

            CarDealerShip carDealerShip = mapper.Map<CarDealershipAddFormModel, CarDealerShip>(dealership);
            carDealerShip.UserId = this.User.Id();

            carDealerShipService.CreateDealerShip(carDealerShip);

            return RedirectToAction();
        }

        public IActionResult CarDealershipAdvertisements()
        {
            return View();
        }
    }
}
