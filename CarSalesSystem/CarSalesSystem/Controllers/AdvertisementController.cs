using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.Advertisement;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.CarDealership;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.ExtrasCategory;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Transmission;
using CarSalesSystem.Services;
using CarSalesSystem.Services.Advertisement;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.CarDealerShip;
using CarSalesSystem.Services.Categories;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;


namespace CarSalesSystem.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly IBrandService brandService;
        private readonly IModelService modelService;
        private readonly ICategoryService categoryService;
        private readonly IColorService colorService;
        private readonly IRegionService regionService;
        private readonly ITechnicalService technicalService;
        private readonly ICarDealerShipService dealerShipService;
        private readonly IAdvertisementService advertisementService;
        private readonly IMapper mapper;

        public AdvertisementController(
            IBrandService brandService,
            IModelService modelService,
            ICategoryService categoryService,
            IMapper mapper, IColorService colorService,
            IRegionService regionService,
            ITechnicalService technicalService,
            ICarDealerShipService dealerShipService,
            IAdvertisementService advertisementService
            )
        {
            this.brandService = brandService;
            this.modelService = modelService;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.colorService = colorService;
            this.regionService = regionService;
            this.technicalService = technicalService;
            this.technicalService = technicalService;
            this.advertisementService = advertisementService;
            this.dealerShipService = dealerShipService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AdvertisementAddFormModel model = new AdvertisementAddFormModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync()),
                VehicleCategories = mapper.Map<ICollection<VehicleCategory>, ICollection<CategoryFormModel>>(await categoryService.GetVehicleCategoriesAsync()),
                Colors = mapper.Map<ICollection<Color>, ICollection<ColorFormModel>>(await colorService.GetColorsAsync()),
                Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(await regionService.GetAllRegionsAsync()),
                EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync()),
                TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync()),
                EuroStandards = mapper.Map<ICollection<VehicleEuroStandard>, ICollection<EuroStandardFormModel>>(await technicalService.GetEuroStandardsAsync()),
                Extras = mapper.Map<ICollection<ExtrasCategory>, ICollection<ExtrasCategoryFormModel>>(await technicalService.GetExtrasCategoriesAsync()),
                Dealerships = mapper.Map<ICollection<CarDealerShip>, ICollection<CarDealershipViewModel>>(await dealerShipService.GetAllCarDealershipsByUserIdAsync(this.User.Id()))
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AdvertisementAddFormModel advertisement)
        {
            if (!ModelState.IsValid)
            {
                return View(advertisement);
            }

            TempData["advertisement" + this.User.Id()] = JsonConvert.SerializeObject(advertisement);

            return RedirectToAction("AddStep2");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddStep2()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddStep2(AdvertisementAddFormModelStep2 advertisementStep2)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0)
                    .Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage });
                return Json(new { IsValid = false, model = advertisementStep2, errors });
            }

            var userId = this.User.Id();
            var key = "advertisement" + userId;

            if (TempData.ContainsKey(key))
            {
                AdvertisementAddFormModel advertisementAddFormModel = JsonConvert.DeserializeObject<AdvertisementAddFormModel>((string)TempData[key]);

                var extrasIdList = new List<string>();

                var advertisementModel = AdvertisementCustomMapper.Map(advertisementAddFormModel, advertisementStep2, userId, extrasIdList);

                string advertisementId = await this.advertisementService.SaveAsync(advertisementModel, extrasIdList, advertisementStep2.Images);

                return Json(new { IsValid = true, redirectToUrl = Url.Action("Details", "Advertisement", new { advertisementId = advertisementId }) });
            }

            var errorMessage = "Error creating advertisement. Please try again.";
            this.ModelState.AddModelError(string.Empty, errorMessage);

            return Json(new { isValid = false, model = advertisementStep2, errorMessage });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            AdvertisementAddFormModel advertisementFormModel = await advertisementService.GetRecordDataAsync(Id, this.User.Id());

            return View(advertisementFormModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(AdvertisementAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["advertisement" + this.User.Id()] = JsonConvert.SerializeObject(model);

            var advertisementId = model.Id;

            return RedirectToAction("EditStep2", new { Id = advertisementId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditStep2(string id)
        {
            AdvertisementAddFormModelStep2 advertisementAddFormModel = await advertisementService.GetRecordDataStep2Async(id);

            return View(advertisementAddFormModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditStep2(AdvertisementAddFormModelStep2 advertisementStep2)
        {
            if (!ModelState.IsValid)
            {
                return View(advertisementStep2);
            }

            var userId = this.User.Id();
            var key = "advertisement" + userId;

            string advertisementId;

            if (TempData.ContainsKey(key))
            {
                AdvertisementAddFormModel advertisementAddFormModel = JsonConvert.DeserializeObject<AdvertisementAddFormModel>((string)TempData[key]);
                advertisementId = advertisementAddFormModel.Id;

                await advertisementService.EditAsync(advertisementAddFormModel, advertisementStep2, userId);
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Error creating advertisement.");
                return View(advertisementStep2);
            }

            return Json(new { redirectToUrl = Url.Action("Details", "Advertisement", new { advertisementId = advertisementId }) });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string advertisementId)
        {
            Advertisement advertisement = await advertisementService.GetAdvertisementByIdAsync(advertisementId);
            AdvertisementViewModel advertisementViewModel = AdvertisementCustomMapper.Map(advertisement);

            return View(advertisementViewModel);
        }

        public JsonResult GetModels(string brandId)
        {
            return Json(modelService.GetAllModels(brandId));
        }

        public JsonResult GetAllCities(string regionId)
        {
            return Json(regionService.GetAllCities(regionId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteAdvertisement(string id)
        {
            try
            {
                await advertisementService.DeleteAsync(id, this.User.Id());

                return RedirectToAction("Index", "Home");
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
