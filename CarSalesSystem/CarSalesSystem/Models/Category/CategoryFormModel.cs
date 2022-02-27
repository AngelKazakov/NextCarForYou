using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Models.Category
{
    public class CategoryFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; init; }

        public string Name { get; init; }
    }
}
