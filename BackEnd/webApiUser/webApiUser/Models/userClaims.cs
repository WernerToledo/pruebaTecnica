namespace webApiUser.Models
{
    public class userClaims
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public bool sessionActive { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Password { get; set; } // Nota: Evitar incluir la contraseña en producción
        public string Direccion { get; set; }
    }
}
