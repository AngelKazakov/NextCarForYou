using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using Microsoft.Extensions.Caching.Memory;

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
        private readonly IMemoryCache memoryCache;

        public SearchController(
            ISearchService searchService,
            IMapper mapper,
            IBrandService brandService,
            ICategoryService categoryService,
            IColorService colorService,
            IRegionService regionService,
            ITechnicalService technicalService, IModelService modelService,
            IMemoryCache memoryCache)
        {
            this.searchService = searchService;
            this.mapper = mapper;
            this.brandService = brandService;
            this.categoryService = categoryService;
            this.colorService = colorService;
            this.regionService = regionService;
            this.technicalService = technicalService;
            this.modelService = modelService;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchAdvertisementModel searchModel)
        {
            ICollection<SearchResultModel> models = await searchService.SearchVehiclesAsync(searchModel);

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> DetailedSearch()
        {
            DetailedSearchAdvertisementModel model = new DetailedSearchAdvertisementModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync()),
                Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(await regionService.GetAllRegionsAsync()),
                TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync()),
                EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync()),
                VehicleCategories = mapper.Map<ICollection<VehicleCategory>, ICollection<CategoryFormModel>>(await categoryService.GetVehicleCategoriesAsync()),
                Colors = mapper.Map<ICollection<Color>, ICollection<ColorFormModel>>(await colorService.GetColorsAsync()),
                EuroStandards = mapper.Map<ICollection<VehicleEuroStandard>, ICollection<EuroStandardFormModel>>(await technicalService.GetEuroStandardsAsync()),
                Extras = mapper.Map<ICollection<ExtrasCategory>, ICollection<ExtrasCategoryFormModel>>(await technicalService.GetExtrasCategoriesAsync()),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DetailedSearch(DetailedSearchAdvertisementModel detailedModel)
        {
            ICollection<SearchResultModel> detailedResultModels = await searchService.DetailedSearchVehiclesAsync(detailedModel);

            return View("Search", detailedResultModels);
        }

        public async Task<IActionResult> SearchMyAdvertisements()
        {
            var cacheKey = "myAdvertisementsCacheKey";

            //checks if cache entries exists
            if (!memoryCache.TryGetValue(cacheKey, out ICollection<SearchResultModel> models))
            {
                //calling the server
                models = await searchService.FindAdvertisementsByUserIdAsync(this.User.Id());

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                //setting cache entries
                memoryCache.Set(cacheKey, models, cacheExpiryOptions);
            }

            return View("Search", models);
        }

        [HttpGet]
        public async Task<IActionResult> AveragePrice()
        {
            AveragePriceModel model = new AveragePriceModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync()),
                Engines = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync()),
                Transmissions = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync()),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AveragePrice(AveragePriceModel priceModel, string command)
        {
            priceModel.Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync());
            priceModel.Engines = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync());
            priceModel.Transmissions = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync());
            priceModel.Models = mapper.Map<ICollection<Model>, ICollection<ModelFormModel>>(await modelService.GetAllModelsAsync(priceModel.Brand));

            if (!ModelState.IsValid)
            {
                return View(priceModel);
            }

            if (priceModel.Year < DataConstants.VehicleMinYear || priceModel.Year > DateTime.UtcNow.Year)
            {
                ModelState.AddModelError(nameof(priceModel.Year), $"Incorrect year. Year should be between {DataConstants.VehicleMinYear} and {DateTime.UtcNow.Year}");
                return View(priceModel);
            }

            priceModel = await searchService.AveragePricesByGivenBrandAndModelAsync(priceModel);

            if (command == "Compare")
            {
                return View(priceModel);
            }

            return View("Search", priceModel.Advertisements);
        }
    }
}
