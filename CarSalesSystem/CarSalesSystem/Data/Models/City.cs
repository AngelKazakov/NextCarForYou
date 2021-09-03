using System;
using System.ComponentModel.DataAnnotations;

using static  CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class City
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(CityNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string RegionId { get; init; }

        public Region Region { get; set; }
    }
}
