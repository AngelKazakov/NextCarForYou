using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Models.Color
{
    public class ColorFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; init; }

        public string Name { get; init; }
    }
}
