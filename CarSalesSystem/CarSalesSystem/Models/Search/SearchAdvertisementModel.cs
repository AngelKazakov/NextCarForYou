using System.Collections.Generic;
using CarSalesSystem.Data.Enums;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Transmission;

namespace CarSalesSystem.Models.Search
{
    public class SearchAdvertisementModel
    {
        public string Brand { get; init; }

        public string Model { get; init; }

        public decimal MaximumPrice { get; init; }

        public int Year { get; init; }

        public string EngineType { get; init; }

        public string TransmissionType { get; init; }

        public string Region { get; init; }

        public string City { get; init; }

        public string OrderBy { get; init; }

        public OrderByValues OrderByValues { get; set; }

        public ICollection<BrandFormModel> Brands { get; init; } = new List<BrandFormModel>();

        public ICollection<RegionFormModel> Regions { get; init; } = new List<RegionFormModel>();

        public ICollection<EngineFormModel> EngineTypes { get; init; } = new List<EngineFormModel>();

        public ICollection<TransmissionFormModel> TransmissionTypes { get; init; } = new List<TransmissionFormModel>();
    }
}
