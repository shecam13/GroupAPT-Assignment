using System.ComponentModel.DataAnnotations;

namespace APTEventAssignment.Models
{
    public class EmailFormModel
    {
        [Required(ErrorMessage="Name is required.")]
        public string FromName { get; set; }

        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string FromEmail { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }
    }
}