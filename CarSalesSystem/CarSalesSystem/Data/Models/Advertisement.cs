﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class Advertisement
    {
        public string Id { get; init; }

        [Required]
        [MaxLength(AdvertisementNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedOnDate { get; init; }

        [Required]
        public DateTime LastModifiedOn { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [MaxLength(AdvertisementDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string VehicleId { get; init; }

        public Vehicle Vehicle { get; set; }

        [Required]
        public string CityId { get; init; }

        public virtual City City { get; init; }

        [Required]
        public string UserId { get; init; }

        [Required]
        public IdentityUser User { get; init; }

        public ICollection<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();

        public ICollection<AdvertisementExtra> AdvertisementExtras { get; init; } = new List<AdvertisementExtra>();

    }
}
