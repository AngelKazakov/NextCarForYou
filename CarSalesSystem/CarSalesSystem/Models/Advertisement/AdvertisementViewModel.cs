using System.Collections.Generic;

namespace CarSalesSystem.Models.Advertisement
{
    public class AdvertisementViewModel
    {
        public string AdvertisementId { get; init; }

        public string Name { get; set; }

        public string CreatedOn { get; init; }

        public string LastModifiedOn { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string VehicleId { get; init; }

        public string ModelId { get; init; }

        public string ModelName { get; set; }

        public int Power { get; init; }

        public string ColorId { get; set; }

        public string ColorName { get; set; }

        public int Mileage { get; set; }

        public int Year { get; set; }

        public string Month { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string EngineTypeId { get; init; }

        public string EngineTypeName { get; set; }

        public string TransmissionTypeId { get; init; }

        public string TransmissionTypeName { get; set; }

        public string EuroStandardId { get; init; }

        public string EuroStandardName { get; set; }

        public string CityId { get; set; }

        public string CityName { get; set; }

        public string RegionId { get; set; }

        public string RegionName { get; set; }

        public string DealershipName { get; set; }

        public string DealershipPhone { get; set; }

        public string UserPhone { get; set; }

        public Dictionary<string, List<string>> Extras { get; set; } = new Dictionary<string, List<string>>();

        public List<byte[]> Images { get; set; } = new List<byte[]>();
    }
}
