using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using CarSalesSystem.Data;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.Search;
using CarSalesSystem.Services.Search;

namespace CarSalesSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        private readonly IMemoryCache memoryCache;

        public SearchController(
            ISearchService searchService,
            IMemoryCache memoryCache)
        {
            this.searchService = searchService;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchAdvertisementModel searchModel)
        {
            ICollection<SearchResultModel> models = await searchService.SearchVehiclesAsync(searchModel, this.User.Id());

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> DetailedSearch()
        {
            return View(await searchService.InitDetailedSearchAdvertisementModelAsync());
        }

        [HttpPost]
        public async Task<IActionResult> DetailedSearch(DetailedSearchAdvertisementModel detailedModel)
        {
            ICollection<SearchResultModel> detailedResultModels = await searchService.DetailedSearchVehiclesAsync(detailedModel, this.User.Id());

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
            return View(await searchService.InitAveragePriceModelAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AveragePrice(AveragePriceModel priceModel, string command)
        {
            if (!ModelState.IsValid)
            {
                return View(priceModel);
            }

            if (priceModel.Year < DataConstants.VehicleMinYear || priceModel.Year > DateTime.UtcNow.Year)
            {
                ModelState.AddModelError(nameof(priceModel.Year), $"Incorrect year. Year should be between {DataConstants.VehicleMinYear} and {DateTime.UtcNow.Year}");
                return View(priceModel);
            }

            priceModel = await searchService.AveragePricesByGivenBrandAndModelAsync(priceModel, this.User.Id());

            if (command == "Compare")
            {
                return View(priceModel);
            }

            return View("Search", priceModel.Advertisements);
        }
    }
}
