using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Models.Engine
{
    public class EngineFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; init; }

        public string Name { get; init; }
    }
}
