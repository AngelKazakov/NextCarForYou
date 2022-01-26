using System.Collections.Generic;
using CarSalesSystem.Data.Enums;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.ExtrasCategory;

namespace CarSalesSystem.Models.Search
{
    public class DetailedSearchAdvertisementModel : SearchAdvertisementModel
    {
        public string Model2 { get; set; }

        public string Brand2 { get; set; }

        public decimal MinPrice { get; set; }

        public string Color { get; set; }

        public string Category { get; set; }

        public string EuroStandard { get; set; }

        public int MinPower { get; set; }

        public int MaxPower { get; set; }

        public int MaxYear { get; set; }

        public int MaxMileage { get; set; }

        public bool AdvertisementsWithImagesOnly { get; set; } = false;

        public AdvertisementOwnerSearchCriteria AdvertisementOwnerSearchCriteria { get; set; }

        public ICollection<CategoryFormModel> VehicleCategories { get; set; } = new List<CategoryFormModel>();

        public ICollection<ColorFormModel> Colors { get; set; } = new List<ColorFormModel>();

        public ICollection<EuroStandardFormModel> EuroStandards { get; set; } = new List<EuroStandardFormModel>();

        public ICollection<ExtrasCategoryFormModel> Extras { get; set; } = new List<ExtrasCategoryFormModel>();

        public ICollection<string> SelectedExtras { get; set; } = new List<string>();
    }
}
