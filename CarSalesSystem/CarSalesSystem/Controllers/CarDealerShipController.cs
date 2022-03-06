using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Exceptions;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.CarDealership;
using CarSalesSystem.Services.CarDealerShip;
using CarSalesSystem.Services.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CarSalesSystem.Controllers
{
    public class CarDealerShipController : Controller
    {
        private const string CacheKey = "myDealershipsCacheKey";

        private readonly ICarDealerShipService carDealerShipService;
        private readonly ISearchService searchService;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;

        public CarDealerShipController(ICarDealerShipService carDealerShipService, IMapper mapper, ISearchService searchService, IMemoryCache memoryCache)
        {
            this.carDealerShipService = carDealerShipService;
            this.mapper = mapper;
            this.searchService = searchService;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateDealership()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDealershipAsync(CarDealershipAddFormModel dealership)
        {
            if (!ModelState.IsValid)
            {
                return View(dealership);
            }

            CarDealerShip carDealerShip = mapper.Map<CarDealershipAddFormModel, CarDealerShip>(dealership);
            carDealerShip.UserId = this.User.Id();

            try
            {
                var dealershipId = await carDealerShipService.CreateDealerShipAsync(carDealerShip);
            }
            catch (DuplicateCarDealerShipException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(dealership);
            }

            TempData["Success"] = $"You successfully created dealership with name '{dealership.Name}'.";

            return RedirectToAction("DealershipsList");
        }

        [HttpGet]
        public async Task<IActionResult> DealershipsList()
        {
            //checks if cache entries exists
            if (!memoryCache.TryGetValue(CacheKey, out ICollection<CarDealershipListingViewModel> dealers))
            {
                dealers = await SetCarDealershipCache(CacheKey);
            }

            return View(dealers);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdvertisementsByDealerId(string dealerId)
        {
            var advertisements = await searchService.BuildSearchResultModelsAsync(await carDealerShipService.GetAdvertisementsByDealershipIdAsync(dealerId));

            return View("../Search/Search", advertisements.ToList());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditCarDealership(string dealerId)
        {
            CarDealershipAddFormModel carDealershipAddFormModel = await carDealerShipService.GetCarDealershipAsync(dealerId);

            //TODO: Fix return view on error
            //if (userId != this.User.Id())
            //{
            //    this.ModelState.AddModelError(string.Empty, "You do not have permission to edit this car dealership.");
            //    return View(carDealershipAddFormModel);
            //}

            return View(carDealershipAddFormModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditCarDealership(CarDealershipAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await carDealerShipService.UpdateCarDealershipAsync(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error updating car dealership.");
                return View(model);
            }

            return RedirectToAction("DealershipsList");

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteCarDealership(string dealerId)
        {
            try
            {
                await carDealerShipService.DeleteCarDealershipAsync(dealerId, this.User.Id());
                await SetCarDealershipCache(CacheKey);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ErrorMessageDealershipDeletion", "Error deleting car dealership.");
                return RedirectToAction("DealershipsList");
            }

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult CarDealershipAdvertisements()
        {
            return View();
        }

        private async Task<ICollection<CarDealershipListingViewModel>> SetCarDealershipCache(string cacheKey)
        {
            //calling the server
            ICollection<CarDealershipListingViewModel> dealers = mapper.Map<ICollection<CarDealerShip>, ICollection<CarDealershipListingViewModel>>
                (await carDealerShipService.GetAllCarDealershipsAsync(), opt => opt.Items["loggedUserId"] = this.User.Id());

            //setting up cache options
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromSeconds(180)
            };
            //setting cache entries
            memoryCache.Set(cacheKey, dealers, cacheExpiryOptions);

            return dealers;
        }
    }
}
