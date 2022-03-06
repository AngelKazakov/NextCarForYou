﻿using System;
using System.Collections.Generic;
using CarSalesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
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
using Microsoft.Extensions.Caching.Memory;

namespace CarSalesSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBrandService brandService;
        private readonly IModelService modelService;
        private readonly IRegionService regionService;
        private readonly ISearchService searchService;
        private readonly ITechnicalService technicalService;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly IEmailSender emailSender;

        public HomeController(
            IBrandService brandService,
            IModelService modelService,
            IRegionService regionService,
            ITechnicalService technicalService,
            ISearchService searchService,
            IMapper mapper,
            IEmailSender emailSender,
            IMemoryCache memoryCache)
        {
            this.emailSender = emailSender;
            this.memoryCache = memoryCache;
            this.searchService = searchService;
            this.brandService = brandService;
            this.modelService = modelService;
            this.regionService = regionService;
            this.technicalService = technicalService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cacheKey = "homeViewModel";

            //checks if cache entries exists
            if (!memoryCache.TryGetValue(cacheKey, out HomeViewModel model))
            {
                //calling the server
                model = new HomeViewModel()
                {
                    SearchAdvertisementModel = new SearchAdvertisementModel()
                    {
                        Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync()),
                        Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(await regionService.GetAllRegionsAsync()),
                        EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync()),
                        TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync()),
                    },
                    LatestPublishedAdvertisements = await searchService.GetLastPublishedAdvertisementsAsync(this.User.Id())
                };

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                //setting cache entries
                memoryCache.Set(cacheKey, model, cacheExpiryOptions);
            }

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
