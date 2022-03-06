using CarSalesSystem.Infrastructure;
using CarSalesSystem.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        => this.userService = userService;

        [HttpPost]
        [Authorize]
        public IActionResult AddAdvertisementToFavorite(string advertisementId)
        {
            userService.AddAdvertisementToFavorite(advertisementId, this.User.Id());

            return RedirectToAction("Index", "Home");
        }
    }
}
