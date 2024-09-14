using Dapper;
using Npgsql;
using System.Data;
using webApiUser.Models;

namespace webApiUser.Services
{
    public class usuarioService
    {
        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor que recibe la configuración a través de la inyección de dependencias
        public usuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresSQLConnection");
        }

        // Propiedad para obtener la conexión a la base de datos
        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        // Método para obtener todos los usuarios
        public async Task<IEnumerable<usuario>> GetAllUsuariosAsync()
        {
            using (var dbConnection = Connection)
            {
                // Ejecutar una consulta SQL para obtener todos los usuarios
                return await dbConnection.QueryAsync<usuario>("SELECT * FROM usuarios");
            }
        }

        // Método para obtener un usuario por su ID
        public async Task<usuario> GetUsuarioByIdAsync(int id)
        {
            using (var dbConnection = Connection)
            {
                // Ejecutar una consulta SQL para obtener un usuario por su ID
                return await dbConnection.QuerySingleOrDefaultAsync<usuario>(
                    "SELECT * FROM usuarios WHERE Id = @Id", new { Id = id });
            }
        }

        // Método para agregar un nuevo usuario
        public async Task<int> AddUsuarioAsync(usuario usuario)
        {
            // Hashear la contraseña del usuario
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.password);
            usuario.password = hashedPassword;

            using (var dbConnection = Connection)
            {
                // Consulta SQL para insertar un nuevo usuario y retornar su ID
                var sql = @"
            INSERT INTO usuarios (nombres, apellidos, fechanacimiento, direccion, password, telefono, email)
            VALUES (@Nombres, @Apellidos, @FechaNacimiento, @Direccion, @Password, @Telefono, @Email)
            RETURNING id";

                // Ejecutar la consulta y obtener el ID del nuevo usuario
                return await dbConnection.ExecuteScalarAsync<int>(sql, new
                {
                    usuario.nombres,
                    usuario.apellidos,
                    usuario.fechanacimiento,
                    usuario.direccion,
                    usuario.password,
                    usuario.telefono,
                    usuario.email
                });
            }
        }

        // Método para actualizar un usuario existente
        public async Task UpdateUsuarioAsync(usuario usuario)
        {
            DateTime fechamodificacion = DateTime.Now;

            // Hashear la nueva contraseña del usuario
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.password);
            usuario.password = hashedPassword;

            using (var dbConnection = Connection)
            {
                // Consulta SQL para actualizar la información del usuario
                var sql = @"
            UPDATE usuarios
            SET nombres = @Nombres,
                apellidos = @Apellidos,
                fechanacimiento = @FechaNacimiento,
                direccion = @Direccion,
                password = @Password,
                telefono = @Telefono,
                email = @Email,
                fechamodificacion = @FechaModificacion
            WHERE id = @Id";

                // Ejecutar la consulta de actualización
                await dbConnection.ExecuteAsync(sql, new
                {
                    usuario.id,
                    usuario.nombres,
                    usuario.apellidos,
                    usuario.fechanacimiento,
                    usuario.direccion,
                    usuario.password,
                    usuario.telefono,
                    usuario.email,
                    fechamodificacion
                });
            }
        }

        // Método para eliminar un usuario por su ID
        public async Task DeleteUsuarioAsync(int id)
        {
            using (var dbConnection = Connection)
            {
                // Consulta SQL para eliminar un usuario por su ID
                var sql = "DELETE FROM usuarios WHERE Id = @Id";
                await dbConnection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
