using System.Collections.Generic;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.Model;
using CarSalesSystem.Models.Transmission;

namespace CarSalesSystem.Models.Search
{
    public class AveragePriceModel
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string EngineType { get; set; }

        public string TransmissionType { get; set; }

        public decimal AveragePrice { get; set; }

        public ICollection<BrandFormModel> Brands { get; set; } = new List<BrandFormModel>();

        public ICollection<ModelFormModel> Models { get; set; } = new List<ModelFormModel>();

        public ICollection<TransmissionFormModel> Transmissions { get; set; } = new List<TransmissionFormModel>();

        public ICollection<EngineFormModel> Engines { get; set; } = new List<EngineFormModel>();

        public ICollection<SearchResultModel> Advertisements { get; set; } = new List<SearchResultModel>();
    }
}
