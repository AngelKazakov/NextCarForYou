using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class CarDealerShip
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(CarDealerShipNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CarDealerAddressMaxLength)]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Url]
        public string Url { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public IdentityUser User { get; set; }

        public DateTime CreatedOn { get; init; }

        public byte[] ImageLogo { get; set; }

        public ICollection<Advertisement> Advertisements { get; init; } = new List<Advertisement>();

    }
}
