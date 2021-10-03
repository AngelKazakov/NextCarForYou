using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Advertisement;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Transmission;
using CarSalesSystem.Services;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.Categories;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IMapper mapper;

        public AdvertisementController(IBrandService brandService, IModelService modelService, ICategoryService categoryService, IMapper mapper, IColorService colorService, IRegionService regionService, ITechnicalService technicalService)
        {
            this.brandService = brandService;
            this.modelService = modelService;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.colorService = colorService;
            this.regionService = regionService;
            this.technicalService = technicalService;
        }

        public IActionResult Add()
        {
            AdvertisementAddFormModel model = new AdvertisementAddFormModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<AddBrandFormModel>>(brandService.GetAllBrands()),
                VehicleCategories = mapper.Map<ICollection<VehicleCategory>, ICollection<AddCategoryFormModel>>(categoryService.GetVehicleCategories()),
                Colors = mapper.Map<ICollection<Color>, ICollection<AddColorFormModel>>(colorService.GetColors()),
                Regions = mapper.Map<ICollection<Region>, ICollection<AddRegionFormModel>>(regionService.GetAllRegions()),
                EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<AddEngineFormModel>>(technicalService.GetEngineTypes()),
                TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<AddTransmissionFormModel>>(technicalService.GetTransmissionTypes()),
                EuroStandards = mapper.Map<ICollection<VehicleEuroStandard>, ICollection<AddEuroStandardFormModel>>(technicalService.GetEuroStandards())

            };

            return View(model);
        }

        public JsonResult GetModels(string brandId)
        {
            return Json(modelService.GetAllModels(brandId));
        }

        public JsonResult GetAllCities(string regionId)
        {
            return Json(regionService.GetAllCities(regionId));
        }

        //public IActionResult AddStep2() => View();

        [HttpPost]
        public IActionResult Add(AdvertisementAddFormModel advertisement)
        {
            return View();
        }

    }
}
