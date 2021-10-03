using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Data.Models
{
    public class Color
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public ICollection<Vehicle> Vehicles { get; init; } = new List<Vehicle>();
    }
}
