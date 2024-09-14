using System.ComponentModel.DataAnnotations;

namespace webApiUser.Models
{
    public class usuario
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(255)]
        public string nombres { get; set; }

        [Required]
        [MaxLength(255)]
        public string apellidos { get; set; }

        [Required]
        public DateTime fechanacimiento { get; set; }

        public string direccion { get; set; }

        [Required]
        [MaxLength(120)]
        public string password { get; set; }

        [Required]
        [MaxLength(9)]
       
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "El teléfono debe estar en el formato ####-####.")]
        public string telefono { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        public string email { get; set; }

    }
}
