using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Models.Model
{
    public class ModelFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
