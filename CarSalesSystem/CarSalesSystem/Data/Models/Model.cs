using System;
using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class Model
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        [Required]
        public string BrandId { get; init; }

        public Brand Brand { get; init; }
    }
}
