using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.CarDealership;
using CarSalesSystem.Services.CarDealerShip;
using CarSalesSystem.Services.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class CarDealerShipController : Controller
    {
        private readonly ICarDealerShipService carDealerShipService;
        private readonly ISearchService searchService;
        private readonly IMapper mapper;

        public CarDealerShipController(ICarDealerShipService carDealerShipService, IMapper mapper, ISearchService searchService)
        {
            this.carDealerShipService = carDealerShipService;
            this.mapper = mapper;
            this.searchService = searchService;
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

        [HttpGet]
        public IActionResult DealershipsList()
        {
            var dealers = mapper.Map<ICollection<CarDealerShip>, ICollection<CarDealershipListingViewModel>>
            (carDealerShipService.GetAllCarDealerships());

            return View(dealers);
        }

        public IActionResult GetAdvertisementsByDealerId(string dealerId)
        {
            var advertisements = searchService.BuildSearchResultModels(carDealerShipService.GetAdvertisementsByDealershipId(dealerId));

            return View("../Search/Search", advertisements.ToList());
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditCarDealership(string dealerId, string userId)
        {
            CarDealershipAddFormModel carDealershipAddFormModel = carDealerShipService.GetCarDealership(dealerId);

            if (userId != this.User.Id())
            {
                this.ModelState.AddModelError(string.Empty, "You do not have permission to edit this car dealership.");
                return View(carDealershipAddFormModel);
            }

            return View(carDealershipAddFormModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditCarDealership(CarDealershipAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //TODO: Check for errors
            carDealerShipService.UpdateCarDealership(model);

            return RedirectToAction();

        }

        public IActionResult CarDealershipAdvertisements()
        {
            return View();
        }
    }
}
