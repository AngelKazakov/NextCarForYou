using System;
using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Data.Models
{
    public class VehicleImage
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        [Required]
        public string UniqueName { get; set; }

        [Required]
        public string FullPath { get; set; }
    }
}
