using System;
using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class Vehicle
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string BrandId { get; set; }

        public Brand Brand { get; init; }

        [Range(0, VehiclePowerMaxValue)]
        public int Power { get; set; }

        public string Color { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        [Required]
        public DateTime Year { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public EngineType EngineType { get; set; }

        [Required]
        public TransmissionType TransmissionType { get; set; }

        public EuroStandard EuroStandard { get; set; }


    }
}
