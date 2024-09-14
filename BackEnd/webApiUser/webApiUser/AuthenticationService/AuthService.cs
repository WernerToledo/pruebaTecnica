using Dapper;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using webApiUser.Models;

namespace webApiUser.AuthenticationService
{
    public class AuthService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresSQLConnection");
            _configuration = configuration;
        }

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public async Task<AuthResponse> Login(Auth autenticacion)
        {
            using (var dbConnection = Connection)
            {
                // Buscar al usuario por teléfono
                var user = await dbConnection.QuerySingleOrDefaultAsync<usuario>(
                    "SELECT * FROM usuarios WHERE telefono = @Telefono", new { Telefono = autenticacion.telefono });

                // Verificar si el usuario existe y si la contraseña es válida
                if (user == null || !BCrypt.Net.BCrypt.Verify(autenticacion.password, user.password))
                {
                    throw new AuthenticationException("Invalid phone number or password.");
                }

                // Generar el token JWT
                var token = GenerateJwtToken(user);

                var UserTokenData = GetTokenClaims(token);
                if (UserTokenData == null)
                {
                    throw new AuthenticationException("Invalid token.");
                }

                var claims = UserTokenData.Claims;
                var usuarioResponse = new userClaims
                {
                    Id = Convert.ToInt32(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
                    Nombres = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value.Split(' ')[0],
                    Apellidos = claims.FirstOrDefault(c => c.Type == "apellidos")?.Value,
                    sessionActive = claims.FirstOrDefault(c => c.Type == "session_activa")?.Value == "true",
                    FechaNacimiento = DateTime.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth)?.Value),
                    Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    Telefono = claims.FirstOrDefault(c => c.Type == "telefono")?.Value,
                    Password = claims.FirstOrDefault(c => c.Type == "password")?.Value,
                    Direccion = claims.FirstOrDefault(c => c.Type == ClaimTypes.StreetAddress)?.Value
                };


                // Crear una respuesta de autenticación
                var response = new AuthResponse
                {
                    User = usuarioResponse,
                    Token = token,
                    TokenType = "Bearer"
                };

                return response; // Retornar la respuesta con el token y los detalles del usuario
            }
        }


        private string GenerateJwtToken(usuario user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.nombres),
                new Claim("apellidos", user.apellidos),
                new Claim("session_activa", "true"),
                new Claim(ClaimTypes.DateOfBirth, user.fechanacimiento.ToString()),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("telefono", user.telefono),
                new Claim("password", user.password),
                new Claim(ClaimTypes.StreetAddress, user.direccion)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetTokenClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
                return principal;
            }
            catch
            {
                // Handle validation failure
                return null;
            }
        }

    }
}
