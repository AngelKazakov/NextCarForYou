using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class Brand
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(VehicleBrandNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Model> Models { get; init; } = new List<Model>();
    }
}
