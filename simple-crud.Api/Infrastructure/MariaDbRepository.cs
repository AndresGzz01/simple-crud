using Dapper;

using simple_crud.Api.Models;
using simple_crud.Library.Models;
using simple_crud.Library.Models.DTOs;

using System.Data.Common;

namespace simple_crud.Api.Infrastructure;

public class MariaDbRepository : IDatabaseRepository
{
    readonly DbConnection dbConnection;
    readonly IPasswordHasher passwordHasher;

    public MariaDbRepository(DbConnection dbConnection, IPasswordHasher passwordHasher)
    {
        this.dbConnection = dbConnection;
        this.passwordHasher = passwordHasher;
    }

    public async Task<OperationResult<Usuario>> CreateUsuario(CreateUsuarioDTO createUsuarioDTO)
    {
        try
        {
            createUsuarioDTO.Username = createUsuarioDTO.Username.Trim();

            await dbConnection.OpenAsync();

            var existQuery = "SELECT COUNT(1) FROM usuario WHERE upper(Username) = @username";
            var exist = await dbConnection.ExecuteScalarAsync<int>(existQuery, new { username = createUsuarioDTO.Username.ToUpper() });

            if (exist > 0)
                return new OperationResult<Usuario>(false, "El nombre de usuario ya existe.");

            var hashedPassword = passwordHasher.HashPassword(createUsuarioDTO.Password);

            var insertQuery = "INSERT INTO usuario (Username, Password) VALUES (@username, @password);";
            var result = await dbConnection.ExecuteAsync(insertQuery, new { username = createUsuarioDTO.Username, password = hashedPassword });

            if (result == 0)
                return new OperationResult<Usuario>(false, "No se pudo crear el usuario.");

            var newUsuarioId = await dbConnection.ExecuteScalarAsync<uint>("SELECT LAST_INSERT_ID();");

            var newUsuario = new Usuario
            {
                Id = newUsuarioId,
                Username = createUsuarioDTO.Username
            };

            return new OperationResult<Usuario>(true, "Usuario creado exitosamente.", newUsuario);
        }
        catch(Exception ex)
        {
            return new OperationResult<Usuario>(false, "Error al crear el usuario.", exception: ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }

    public async Task<OperationResult<Usuario?>> GetUsuarioById(uint id)
    {
        try
        {
            await dbConnection.OpenAsync();

            var query = "SELECT Id, Username FROM usuario WHERE Id = @id LIMIT 1";
            var usuario = await dbConnection.QuerySingleOrDefaultAsync<Usuario>(query, new { id });

            return new OperationResult<Usuario?>(true, value: usuario);
        }
        catch (Exception ex)
        {
            return new OperationResult<Usuario?>(false, $"Error al obtener el usuario con Id {id}.", exception: ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }

    public async Task<OperationResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        try
        {
            await dbConnection.OpenAsync();

            var query = "SELECT Id, Username FROM usuario";
            var usuarios = await dbConnection.QueryAsync<Usuario>(query);

            return new OperationResult<IEnumerable<Usuario>>(true, value: usuarios);
        }
        catch(Exception ex)
        {
            return new OperationResult<IEnumerable<Usuario>>(false, "Error al obtener los usuarios.", [], ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }
}
