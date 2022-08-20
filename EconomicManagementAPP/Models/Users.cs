using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string StandarEmail { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Password { get; set; }
    }
}
