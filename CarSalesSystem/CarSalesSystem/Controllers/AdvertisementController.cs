using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.Advertisement;
using CarSalesSystem.Models.Brand;
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
using CarSalesSystem.Services.Categories;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IAdvertisementService advertisementService;
        private readonly IMapper mapper;

        public AdvertisementController(
            IBrandService brandService,
            IModelService modelService,
            ICategoryService categoryService,
            IMapper mapper, IColorService colorService,
            IRegionService regionService,
            ITechnicalService technicalService,
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
            this.advertisementService = advertisementService;
        }

        [Authorize]
        public IActionResult Add()
        {
            AdvertisementAddFormModel model = new AdvertisementAddFormModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(brandService.GetAllBrands()),
                VehicleCategories = mapper.Map<ICollection<VehicleCategory>, ICollection<CategoryFormModel>>(categoryService.GetVehicleCategories()),
                Colors = mapper.Map<ICollection<Color>, ICollection<ColorFormModel>>(colorService.GetColors()),
                Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(regionService.GetAllRegions()),
                EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(technicalService.GetEngineTypes()),
                TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(technicalService.GetTransmissionTypes()),
                EuroStandards = mapper.Map<ICollection<VehicleEuroStandard>, ICollection<EuroStandardFormModel>>(technicalService.GetEuroStandards()),
                Extras = mapper.Map<ICollection<ExtrasCategory>, ICollection<ExtrasCategoryFormModel>>(technicalService.GetExtrasCategories())
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AdvertisementAddFormModel advertisement)
        {
            TempData["advertisement" + this.User.Id()] = JsonConvert.SerializeObject(advertisement);

            return RedirectToAction("AddStep2");
        }

        [Authorize]
        public IActionResult AddStep2()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddStep2(AdvertisementAddFormModelStep2 advertisementStep2)
        {
            if (!ModelState.IsValid)
            {
                return View(advertisementStep2);
            }

            var userId = this.User.Id();
            var key = "advertisement" + userId;

            if (TempData.ContainsKey(key))
            {
                AdvertisementAddFormModel advertisementAddFormModel = JsonConvert.DeserializeObject<AdvertisementAddFormModel>((string)TempData[key]);

                var extrasIdList = new List<string>();

                var advertisementModel = AdvertisementCustomMapper.Map(advertisementAddFormModel, advertisementStep2, userId, extrasIdList);

                this.advertisementService.Save(advertisementModel, extrasIdList, advertisementStep2.Images);
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Error creating advertisement.");
                return View(advertisementStep2);
            }


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Details(string advertisementId)
        {
            return View(advertisementService.GetAdvertisementById(advertisementId));
        }

        public JsonResult GetModels(string brandId)
        {
            return Json(modelService.GetAllModels(brandId));
        }

        public JsonResult GetAllCities(string regionId)
        {
            return Json(regionService.GetAllCities(regionId));
        }
    }
}
