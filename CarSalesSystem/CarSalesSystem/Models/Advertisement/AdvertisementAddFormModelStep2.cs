using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Models.Advertisement
{
    public class AdvertisementAddFormModelStep2
    {
        public string Id { get; init; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(
            AdvertisementDescriptionMaxLength,
            MinimumLength = AdvertisementDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }

        public ICollection<IFormFile> Images { get; set; } = new List<IFormFile>();

        public Dictionary<string, byte[]> ImagesForDisplay { get; set; } = new Dictionary<string, byte[]>();

        public string ImagesForDeletion { get; set; }
    }
}
