using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class RegisterViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "Invalid format Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
