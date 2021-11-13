using System.Collections.Generic;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Advertisement;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Search;
using CarSalesSystem.Models.Transmission;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
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
        private readonly IMapper mapper;

        public SearchController(
            IBrandService brandService,
            IModelService modelService,
            IRegionService regionService,
            ITechnicalService technicalService,
            IMapper mapper)
        {
            this.brandService = brandService;
            this.modelService = modelService;
            this.regionService = regionService;
            this.technicalService = technicalService;
            this.mapper = mapper;
        }

        public IActionResult SearchResult()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(SearchAdvertisementModel searchModel)
        {
            return View();
        }
    }
}
