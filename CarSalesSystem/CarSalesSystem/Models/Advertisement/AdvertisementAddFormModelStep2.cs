using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Models.Advertisement
{
    public class AdvertisementAddFormModelStep2
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(
            AdvertisementDescriptionMaxLength,
            MinimumLength = AdvertisementDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }

        [Required]
        public IFormFileCollection Images { get; set; }
    }
}
