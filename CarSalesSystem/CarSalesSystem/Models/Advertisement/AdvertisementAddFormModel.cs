using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.CarDealership;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.City;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.ExtrasCategory;
using CarSalesSystem.Models.Model;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Transmission;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Models.Advertisement
{
    public class AdvertisementAddFormModel
    {
        public string Id { get; init; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public BrandFormModel BrandFormModel { get; set; }

        [Required]
        public ModelFormModel ModelFormModel { get; set; }

        [Required]
        public int Power { get; set; }

        [Required]
        public ColorFormModel ColorFormModel { get; set; }

        [Required]
        public int Mileage { get; set; }

        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public CategoryFormModel CategoryFormModel { get; set; }

        [Required]
        public EngineFormModel EngineTypeFormModel { get; set; }

        [Required]
        public TransmissionFormModel TransmissionTypeFormModel { get; set; }

        public EuroStandardFormModel EuroStandardFormModel { get; set; }

        [Required]
        public RegionFormModel RegionFormModel { get; set; }

        [Required]
        public CityFormModel CityFormModel { get; set; }

        public CarDealershipFormModel DealershipFormModel { get; set; }

        public ICollection<ExtrasCategoryFormModel> Extras { get; set; } = new List<ExtrasCategoryFormModel>();

        [BindProperty]
        public ICollection<string> SelectedExtras { get; set; } = new List<string>();

        public ICollection<BrandFormModel> Brands { get; init; } = new List<BrandFormModel>();

        public ICollection<CategoryFormModel> VehicleCategories { get; init; } = new List<CategoryFormModel>();

        public ICollection<EuroStandardFormModel> EuroStandards { get; init; } = new List<EuroStandardFormModel>();

        public ICollection<ColorFormModel> Colors { get; init; } = new List<ColorFormModel>();

        public ICollection<RegionFormModel> Regions { get; init; } = new List<RegionFormModel>();

        public ICollection<EngineFormModel> EngineTypes { get; init; } = new List<EngineFormModel>();

        public ICollection<TransmissionFormModel> TransmissionTypes { get; init; } = new List<TransmissionFormModel>();

        public ICollection<ModelFormModel> Models { get; set; } = new List<ModelFormModel>();

        public ICollection<CityFormModel> Cities { get; set; } = new List<CityFormModel>();

        public ICollection<CarDealershipViewModel> Dealerships { get; set; } = new List<CarDealershipViewModel>();
    }
}
