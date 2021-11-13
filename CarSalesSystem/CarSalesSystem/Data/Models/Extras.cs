using System;
using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class Extras
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(ExtrasNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string CategoryId { get; init; }

        
        public ExtrasCategory Category { get; init; }
    }
}
