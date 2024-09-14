using Dapper;
using Npgsql;
using System.Data;
using webApiUser.Models;

namespace webApiUser.Services
{
    public class usuarioService
    {
        private readonly string _connectionString;

        public usuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresSQLConnection");
        }

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<usuario>> GetAllUsuariosAsync()
        {
            using (var dbConnection = Connection)
            {
                return await dbConnection.QueryAsync<usuario>("SELECT * FROM usuarios");
            }
        }

        public async Task<usuario> GetUsuarioByIdAsync(int id)
        {
            using (var dbConnection = Connection)
            {
                return await dbConnection.QuerySingleOrDefaultAsync<usuario>(
                    "SELECT * FROM usuarios WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<int> AddUsuarioAsync(usuario usuario)
        {
            using (var dbConnection = Connection)
            {
                var sql = @"
            INSERT INTO usuarios (nombres, apellidos, fechanacimiento, direccion, password, telefono, email)
            VALUES (@Nombres, @Apellidos, @FechaNacimiento, @Direccion, @Password, @Telefono, @Email)
            RETURNING id";

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

        public async Task UpdateUsuarioAsync(usuario usuario)
        {
            DateTime fechamodificacion = DateTime.Now;

            using (var dbConnection = Connection)
            {
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


        public async Task DeleteUsuarioAsync(int id)
        {
            using (var dbConnection = Connection)
            {
                var sql = "DELETE FROM usuarios WHERE Id = @Id";
                await dbConnection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
