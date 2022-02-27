using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using CarSalesSystem.Data;

namespace CarSalesSystem.Models.Advertisement
{
    public class AdvertisementAddFormModelStep2
    {
        public string Id { get; init; }

        [Required(ErrorMessage = DataConstants.ErrorMessageRequiredField)]
        public string Name { get; set; }

        [Required(ErrorMessage = DataConstants.ErrorMessageRequiredField)]
        [StringLength(
           DataConstants.AdvertisementDescriptionMaxLength,
            MinimumLength = DataConstants.AdvertisementDescriptionMinLength,
            ErrorMessage = DataConstants.ErrorMessageDescriptionField)]
        public string Description { get; set; }

        public ICollection<IFormFile> Images { get; set; } = new List<IFormFile>();

        public Dictionary<string, byte[]> ImagesForDisplay { get; set; } = new Dictionary<string, byte[]>();

        public string ImagesForDeletion { get; set; }
    }
}
