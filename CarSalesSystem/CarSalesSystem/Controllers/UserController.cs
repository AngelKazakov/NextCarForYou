using CarSalesSystem.Infrastructure;
using CarSalesSystem.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        => this.userService = userService;

        [HttpPost]
        public JsonResult AddAdvertisementToFavorite(string advertisementId)
        {
            if (userService.AddAdvertisementToFavorite(advertisementId, this.User.Id()))
            {
                return Json("Ok");
            }

            return Json(new { Success = false });
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
