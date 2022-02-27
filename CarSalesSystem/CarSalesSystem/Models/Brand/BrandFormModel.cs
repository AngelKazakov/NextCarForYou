using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CarSalesSystem.Models.Brand
{
    public class BrandFormModel
    {
        [Required(ErrorMessage = Data.DataConstants.ErrorMessageRequiredField)]
        public string Id { get; init; }

        public string Name { get; init; }
    }
}
