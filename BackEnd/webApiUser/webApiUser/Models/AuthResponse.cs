namespace webApiUser.Models
{
    public class AuthResponse
    {
        public userClaims User { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; }
    }
}
