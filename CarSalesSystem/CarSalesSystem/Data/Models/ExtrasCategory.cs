using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class ExtrasCategory
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Extras> Extras { get; init; } = new List<Extras>();
    }
}
