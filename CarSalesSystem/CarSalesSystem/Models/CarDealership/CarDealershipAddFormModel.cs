using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Models.CarDealership
{
    public class CarDealershipAddFormModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(CarDealerShipNameMaxLength)]
        public string Name { get; init; }

        [Required]
        public string Address { get; init; }

        [Required]
        [Phone]
        public string Phone { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Url]
        public string Url { get; init; }

        public IFormFile Image { get; init; }

        public string ImageId { get; init; }

        public byte[] ImageForDisplay { get; init; }
    }
}
