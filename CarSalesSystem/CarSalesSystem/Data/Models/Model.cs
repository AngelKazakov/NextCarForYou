using System;
using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Data.Models
{
    public class Model
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public string BrandId { get; init; }

        public Brand Brand { get; init; }
    }
}
