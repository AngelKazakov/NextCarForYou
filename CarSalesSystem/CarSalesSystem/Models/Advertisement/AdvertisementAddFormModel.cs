using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Transmission;

namespace CarSalesSystem.Models.Advertisement
{
    public class AdvertisementAddFormModel
    {
        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Power { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public int Mileage { get; set; }

        public string Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string EngineType { get; set; }

        [Required]
        public string TransmissionType { get; set; }

        public string EuroStandard { get; set; }

        [Required]
        public string RegionName { get; set; }

        [Required]
        public string CityName { get; set; }

        public ICollection<string> Extras { get; init; } = new List<string>();

        public ICollection<string> SelectedExtras { get; init; } = new List<string>();

        public ICollection<AddBrandFormModel> Brands { get; init; } = new List<AddBrandFormModel>();

        public ICollection<AddCategoryFormModel> VehicleCategories { get; init; } = new List<AddCategoryFormModel>();

        public ICollection<AddEuroStandardFormModel> EuroStandards { get; init; } = new List<AddEuroStandardFormModel>();

        public ICollection<AddColorFormModel> Colors { get; init; } = new List<AddColorFormModel>();

        public ICollection<AddRegionFormModel> Regions { get; init; } = new List<AddRegionFormModel>();

        public ICollection<AddEngineFormModel> EngineTypes { get; init; } = new List<AddEngineFormModel>();

        public ICollection<AddTransmissionFormModel> TransmissionTypes { get; init; } = new List<AddTransmissionFormModel>();

        //TODO: Add images...

    }
}
