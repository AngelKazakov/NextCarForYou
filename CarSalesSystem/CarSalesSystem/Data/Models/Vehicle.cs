using System;
using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class Vehicle
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string ModelId { get; set; }

        public Model Model { get; init; }

        [Range(0, VehiclePowerMaxValue)]
        public int Power { get; set; }

        public string ColorId { get; set; }

        public Color Color { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        public string CategoryId { get; set; }

        [Required]
        public VehicleCategory Category { get; set; }

        public string EngineTypeId { get; set; }

        [Required]
        public VehicleEngineType EngineType { get; set; }

        public string TransmissionTypeId { get; set; }

        [Required]
        public TransmissionType TransmissionType { get; set; }

        public string EuroStandardId { get; set; }

        public VehicleEuroStandard EuroStandard { get; set; }
    }
}
