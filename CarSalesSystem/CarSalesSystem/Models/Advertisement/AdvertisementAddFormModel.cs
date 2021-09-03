using System;

namespace CarSalesSystem.Models.Advertisement
{
    public class AdvertisementAddFormModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Power { get; set; }

        public string Color { get; set; }

        public int Mileage { get; set; }

        public DateTime Year { get; set; }

        public string Category { get; set; }

        public string EngineType { get; set; }

        public string TransmissionType { get; set; }

        public string EuroStandard { get; set; }

        public string RegionName { get; set; }

        public string CityName { get; set; }

        //TODO: Add images...

    }
}
