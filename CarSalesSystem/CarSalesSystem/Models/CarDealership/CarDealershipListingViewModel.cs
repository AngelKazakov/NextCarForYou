using System;

namespace CarSalesSystem.Models.CarDealership
{
    public class CarDealershipListingViewModel
    {
        public string Id { get; init; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public bool IsAllowedToEdit { get; set; }

        public DateTime CreatedOn { get; set; }

        public byte[] ImageLogo { get; set; }
    }
}
