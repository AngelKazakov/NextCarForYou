using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class Region
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(RegionNameMaxLength)]
        public string Name { get; set; }

        public ICollection<City> Cities { get; init; } = new List<City>();
    }
}
