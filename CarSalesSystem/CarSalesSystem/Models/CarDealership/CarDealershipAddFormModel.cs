using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Models.CarDealership
{
    public class CarDealershipAddFormModel
    {
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
    }
}
