using System.Collections.Generic;
using CarSalesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure.EmailConfiguration;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Contact;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.Home;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Search;
using CarSalesSystem.Models.Transmission;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.Email;
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
        private readonly IEmailSender emailSender;

        public HomeController(
            IBrandService brandService,
            IModelService modelService,
            IRegionService regionService,
            ITechnicalService technicalService,
            ISearchService searchService,
            IMapper mapper, ILogger<HomeController> logger,
            IEmailSender emailSender)
        {
            _logger = logger;
            this.emailSender = emailSender;
            this.searchService = searchService;
            this.brandService = brandService;
            this.modelService = modelService;
            this.regionService = regionService;
            this.technicalService = technicalService;
            this.mapper = mapper;
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormView contactModel)
        {
            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }

            var message = new Message(new string[] { "drake166@abv.bg" }, "About", contactModel.Message)
            {
                SenderName = contactModel.SenderName,
                SenderEmail = contactModel.Email,
                SenderPhone = contactModel.PhoneNumber
            };
            await emailSender.SendEmailAsync(message);

            TempData["Success"] = "Thank you for contacting us. We will get in touch with you soon.";

            return RedirectToAction("Contact");
        }

        [HttpGet]
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
