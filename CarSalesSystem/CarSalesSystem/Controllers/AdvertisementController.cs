using CarSalesSystem.Models.Advertisement;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Controllers
{
    public class AdvertisementController : Controller
    {
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(AdvertisementAddFormModel advertisement)
        {
            return View();
        }

    }
}
