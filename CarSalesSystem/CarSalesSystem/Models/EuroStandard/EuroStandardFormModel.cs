using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Models.EuroStandard
{
    public class EuroStandardFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; init; }

        public string Name { get; init; }
    }
}
