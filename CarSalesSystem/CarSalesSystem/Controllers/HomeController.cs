using System;
using CarSalesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Infrastructure.EmailConfiguration;
using CarSalesSystem.Models.Contact;
using CarSalesSystem.Models.Home;
using CarSalesSystem.Services.Email;
using CarSalesSystem.Services.Search;

namespace CarSalesSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchService searchService;
        private readonly IMemoryCache memoryCache;
        private readonly IEmailSender emailSender;

        public HomeController(
            ISearchService searchService,
            IEmailSender emailSender,
            IMemoryCache memoryCache)
        {
            this.emailSender = emailSender;
            this.memoryCache = memoryCache;
            this.searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cacheKey = "homeViewModel";

            //checks if cache entries exists
            if (!memoryCache.TryGetValue(cacheKey, out HomeViewModel model))
            {
                //calling the server
                model = await searchService.InitHomeViewModelAsync(this.User.Id());

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(10),
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
