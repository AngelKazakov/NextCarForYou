﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Data.Models
{
    public class CarDealerShip
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(CarDealerShipNameMaxLength)]
        public  string Name { get; init; }
      
        [Required]
        [MaxLength(CarDealerAddressMaxLength)]
        public  string Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Url]
        public string Url { get; set; }

        public ICollection<Advertisement> Advertisements { get; init; } = new List<Advertisement>();

    }
}