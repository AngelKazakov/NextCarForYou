using System.Collections.Generic;
using CarSalesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.Home;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Search;
using CarSalesSystem.Models.Transmission;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.Search;
using CarSalesSystem.Services.TechnicalData;

namespace CarSalesSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBrandService brandService;
        private readonly IModelService modelService;
        private readonly IRegionService regionService;
        private readonly ISearchService searchService;
        private readonly ITechnicalService technicalService;
        private readonly IMapper mapper;

        public HomeController(
            IBrandService brandService,
            IModelService modelService,
            IRegionService regionService,
            ITechnicalService technicalService,
            ISearchService searchService,
            IMapper mapper, ILogger<HomeController> logger)
        {
            _logger = logger;
            this.searchService = searchService;
            this.brandService = brandService;
            this.modelService = modelService;
            this.regionService = regionService;
            this.technicalService = technicalService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel()
            {
                SearchAdvertisementModel = new SearchAdvertisementModel()
                {
                    Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(brandService.GetAllBrands()),
                    Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(regionService.GetAllRegions()),
                    EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(technicalService.GetEngineTypes()),
                    TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(technicalService.GetTransmissionTypes()),
                },
                LatestPublishedAdvertisements = searchService.GetLastPublishedAdvertisements()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel homeViewModel)
        {
            return RedirectToAction("Search", "Search", homeViewModel.SearchAdvertisementModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
