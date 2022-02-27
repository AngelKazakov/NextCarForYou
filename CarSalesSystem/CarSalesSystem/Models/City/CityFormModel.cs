using System.ComponentModel.DataAnnotations;

namespace CarSalesSystem.Models.City
{
    public class CityFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
