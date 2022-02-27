using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Models.Transmission
{
    public class TransmissionFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; init; }

        public string Name { get; init; }
    }
}
