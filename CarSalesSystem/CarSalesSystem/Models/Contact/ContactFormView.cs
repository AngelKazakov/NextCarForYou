using System.ComponentModel.DataAnnotations;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Models.Contact
{
    public class ContactFormView
    {
        [Required(ErrorMessage = ErrorMessageRequiredField)]
        public string SenderName { get; set; }

        [Required(ErrorMessage = ErrorMessageRequiredField)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMessageRequiredField)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = ErrorMessageRequiredField)]
        public string Message { get; set; }
    }
}
