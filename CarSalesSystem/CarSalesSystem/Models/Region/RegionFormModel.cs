using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Models.Region
{
    public class RegionFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; init; }

        public string Name { get; init; }
    }
}
