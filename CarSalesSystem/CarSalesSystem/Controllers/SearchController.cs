using System;
using System.Collections.Generic;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.ExtrasCategory;
using CarSalesSystem.Models.Model;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Search;
using CarSalesSystem.Models.Transmission;
using CarSalesSystem.Services;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.Categories;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.Search;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly IMapper mapper;
        private readonly ISearchService searchService;
        private readonly IModelService modelService;
        private readonly IBrandService brandService;
        private readonly ICategoryService categoryService;
        private readonly IColorService colorService;
        private readonly IRegionService regionService;
        private readonly ITechnicalService technicalService;

        public SearchController(
            ISearchService searchService,
            IMapper mapper,
            IBrandService brandService,
            ICategoryService categoryService,
            IColorService colorService,
            IRegionService regionService,
            ITechnicalService technicalService, IModelService modelService)
        {
            this.searchService = searchService;
            this.mapper = mapper;
            this.brandService = brandService;
            this.categoryService = categoryService;
            this.colorService = colorService;
            this.regionService = regionService;
            this.technicalService = technicalService;
            this.modelService = modelService;
        }

        [HttpGet]
        public IActionResult Search(SearchAdvertisementModel searchModel)
        {
            ICollection<SearchResultModel> models = searchService.SearchVehicles(searchModel);

            return View(models);
        }

        [HttpGet]
        public IActionResult DetailedSearch()
        {
            DetailedSearchAdvertisementModel model = new DetailedSearchAdvertisementModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(brandService.GetAllBrands()),
                VehicleCategories = mapper.Map<ICollection<VehicleCategory>, ICollection<CategoryFormModel>>(categoryService.GetVehicleCategories()),
                Colors = mapper.Map<ICollection<Color>, ICollection<ColorFormModel>>(colorService.GetColors()),
                Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(regionService.GetAllRegions()),
                EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(technicalService.GetEngineTypes()),
                TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(technicalService.GetTransmissionTypes()),
                EuroStandards = mapper.Map<ICollection<VehicleEuroStandard>, ICollection<EuroStandardFormModel>>(technicalService.GetEuroStandards()),
                Extras = mapper.Map<ICollection<ExtrasCategory>, ICollection<ExtrasCategoryFormModel>>(technicalService.GetExtrasCategories()),
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult DetailedSearch(DetailedSearchAdvertisementModel detailedModel)
        {
            ICollection<SearchResultModel> detailedResultModels = searchService.DetailedSearchVehicles(detailedModel);

            return View("Search", detailedResultModels);
        }

        public IActionResult SearchMyAdvertisements()
        {
            ICollection<SearchResultModel> models = searchService.FindAdvertisementsByUserId(this.User.Id());

            return View("Search", models);
        }

        [HttpGet]
        public IActionResult AveragePrice()
        {
            AveragePriceModel model = new AveragePriceModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(brandService.GetAllBrands()),
                Engines = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(technicalService.GetEngineTypes()),
                Transmissions = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(technicalService.GetTransmissionTypes()),
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AveragePrice(AveragePriceModel priceModel, string command)
        {
            priceModel.Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(brandService.GetAllBrands());
            priceModel.Engines = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(technicalService.GetEngineTypes());
            priceModel.Transmissions = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(technicalService.GetTransmissionTypes());
            priceModel.Models = mapper.Map<ICollection<Model>, ICollection<ModelFormModel>>(modelService.GetAllModels(priceModel.Brand));

            if (!ModelState.IsValid)
            {
                return View(priceModel);
            }

            if (priceModel.Year < 1930 || priceModel.Year > DateTime.UtcNow.Year)
            {
                ModelState.AddModelError(nameof(priceModel.Year), $"Incorrect year. Year should be between 1930 and {DateTime.UtcNow.Year}");
                return View(priceModel);
            }

            priceModel = searchService.AveragePricesByGivenBrandAndModel(priceModel);

            if (command == "Compare")
            {
                return View(priceModel);
            }

            return View("Search", priceModel.Advertisements);
        }
    }
}
