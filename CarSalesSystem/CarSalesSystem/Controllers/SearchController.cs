using System.Collections.Generic;
using System.Linq;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.Search;
using CarSalesSystem.Services.Search;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(
            ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public IActionResult Search(SearchAdvertisementModel searchModel)
        {
            List<SearchResultModel> models = searchService.SearchVehicles(searchModel);

            return View(models);
        }

        public IActionResult SearchMyAdvertisements()
        {
            var models = searchService.FindAdvertisementsByUserId(this.User.Id());

            return View("Search", models.ToList());
        }
    }
}
