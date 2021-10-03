using System;
using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Data.Models
{
    public class TransmissionType
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }
    }
}
