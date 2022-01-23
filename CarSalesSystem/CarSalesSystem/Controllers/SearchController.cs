using System.Collections.Generic;
using AutoMapper;
using CarSalesSystem.Models.Search;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.Search;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly IBrandService brandService;
        private readonly IModelService modelService;
        private readonly IRegionService regionService;
        private readonly ITechnicalService technicalService;
        private readonly ISearchService searchService;
        private readonly IMapper mapper;

        public SearchController(
            IBrandService brandService,
            IModelService modelService,
            IRegionService regionService,
            ITechnicalService technicalService,
            IMapper mapper, ISearchService searchService)
        {
            this.brandService = brandService;
            this.modelService = modelService;
            this.regionService = regionService;
            this.technicalService = technicalService;
            this.mapper = mapper;
            this.searchService = searchService;
        }

        [HttpGet]
        public IActionResult SearchResult()
        {
            return View();
        }

        //[HttpPost]
        public IActionResult Search(SearchAdvertisementModel searchModel)
        {
            List<SearchResultModel> models = searchService.SearchVehicles(searchModel);

            return View(models);
        }
    }
}
