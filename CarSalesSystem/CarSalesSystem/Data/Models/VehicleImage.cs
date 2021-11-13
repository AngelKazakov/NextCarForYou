using System;
using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Data.Models
{
    public class VehicleImage
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string UniqueName { get; set; }

        public string FullPath { get; set; }

    }
}
