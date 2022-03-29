using System.Threading.Tasks;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Services.Search;
using CarSalesSystem.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly ISearchService searchService;

        public UserController(IUserService userService, ISearchService searchService)
        {
            this.userService = userService;
            this.searchService = searchService;
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> AddAdvertisementToFavorite(string advertisementId)
        {
            var result = await userService.AddAdvertisementToFavoriteAsync(advertisementId, this.User.Id());

            return Json(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FavoriteAdvertisements()
        {
            return View(await searchService.GetUserFavoriteAdvertisementsAsync(this.User.Id()));
        }
    }
}
