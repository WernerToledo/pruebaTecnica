using System.ComponentModel.DataAnnotations;

namespace webApiUser.Models
{
    public class Auth
    {
        [Required]
        public string telefono { get; set; }

        [Required]
        public string password { get; set; }
    }
}
