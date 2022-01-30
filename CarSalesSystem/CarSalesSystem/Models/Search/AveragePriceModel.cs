using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.Model;
using CarSalesSystem.Models.Transmission;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Models.Search
{
    public class AveragePriceModel
    {
        [Required(ErrorMessage = ErrorMessageRequiredField)]
        public string Brand { get; init; }

        [Required(ErrorMessage = ErrorMessageRequiredField)]
        public string Model { get; init; }

        [Required(ErrorMessage = ErrorMessageRequiredField)]
        public int Year { get; init; }

        [Required(ErrorMessage = ErrorMessageRequiredField)]
        public string EngineType { get; init; }

        [Required(ErrorMessage = ErrorMessageRequiredField)]
        public string TransmissionType { get; init; }

        public decimal AveragePrice { get; set; }

        public ICollection<BrandFormModel> Brands { get; set; } = new List<BrandFormModel>();

        public ICollection<ModelFormModel> Models { get; set; } = new List<ModelFormModel>();

        public ICollection<TransmissionFormModel> Transmissions { get; set; } = new List<TransmissionFormModel>();

        public ICollection<EngineFormModel> Engines { get; set; } = new List<EngineFormModel>();

        public ICollection<SearchResultModel> Advertisements { get; set; } = new List<SearchResultModel>();
    }
}
