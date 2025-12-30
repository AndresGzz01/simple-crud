using Dapper;

using simple_crud.Api.Models;
using simple_crud.Library.Models;
using simple_crud.Library.Models.DTOs;

using System.Data;
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
        catch (Exception ex)
        {
            return new OperationResult<Usuario>(false, "Error al crear el usuario.", exception: ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }

    public async Task<OperationResult> DeleteUsuario(DeleteUsuarioDTO deleteUsuarioDTO)
    {
        try
        {
            await dbConnection.OpenAsync();

            var query = "SELECT Password FROM usuario WHERE Id = @id LIMIT 1";
            var storedPassword = await dbConnection.ExecuteScalarAsync<string?>(query, new { deleteUsuarioDTO.Id });

            if (storedPassword is null
                || !passwordHasher.VerifyPassword(storedPassword, deleteUsuarioDTO.Password))
            {
                return new OperationResult(false, "Credenciales inválidas.");
            }

            var deleteQuery = "DELETE FROM usuario WHERE Id = @id";
            var result = await dbConnection.ExecuteAsync(deleteQuery, new { deleteUsuarioDTO.Id });

            if (result == 0)
                return new OperationResult(false, "No se pudo eliminar el usuario.");

            return new OperationResult(true, "Usuario eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return new OperationResult(false, "Error al eliminar el usuario.", exception: ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }

    public async Task<OperationResult<IEnumerable<Post>>> GetPostsByUsuarioId(uint usuarioId)
    {
        try
        {
            dbConnection.Open();

            var query = "SELECT Id, Titulo, UltimaActualizacion, IdUsuario FROM post WHERE IdUsuario = @usuarioId";
            var posts = await dbConnection.QueryAsync<Post>(query, new { usuarioId });

            return new OperationResult<IEnumerable<Post>>(true, value: posts);
        }
        catch (Exception ex)
        {
            return new OperationResult<IEnumerable<Post>>(false, $"Error al obtener los posts del usuario con Id {usuarioId}.", exception: ex);
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
            if (dbConnection.State != ConnectionState.Open)
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
        catch (Exception ex)
        {
            return new OperationResult<IEnumerable<Usuario>>(false, "Error al obtener los usuarios.", [], ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }

    public async Task<OperationResult<Usuario?>> LoginUsuario(LoginUsuarioDTO loginUsuarioDTO)
    {
        try
        {
            await dbConnection.OpenAsync();

            var query = "SELECT Id, Username, Password FROM usuario WHERE upper(Username) = @username LIMIT 1";
            var usuario = await dbConnection.QuerySingleOrDefaultAsync<Usuario>(query, new { username = loginUsuarioDTO.Username.ToUpper() });

            if (usuario is null
                || !passwordHasher.VerifyPassword(usuario.Password!, loginUsuarioDTO.Password))
            {
                return new OperationResult<Usuario?>(false, "Credenciales inválidas.");
            }

            return new OperationResult<Usuario?>(true, "Inicio de sesión exitoso.", new Usuario
            {
                Id = usuario.Id,
                Username = usuario.Username
            });
        }
        catch (Exception ex)
        {
            return new OperationResult<Usuario?>(false, "Error al iniciar sesión.", exception: ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }

    public async Task<OperationResult<Usuario?>> UpdateUsuario(UpdateUsuarioDTO updateUsuarioDTO)
    {
        try
        {
            await dbConnection.OpenAsync();

            var query = "SELECT Id, Username, Password FROM usuario WHERE Id = @id LIMIT 1";
            var usuarioToUpdate = await dbConnection.QuerySingleOrDefaultAsync<Usuario?>(query, new { id = updateUsuarioDTO.Id });

            if (usuarioToUpdate is null)
                return new OperationResult<Usuario?>(false, "El usuario no existe.");

            var isValidPassord = passwordHasher.VerifyPassword(usuarioToUpdate.Password!, updateUsuarioDTO.Password);
            if (!isValidPassord)
                return new OperationResult<Usuario?>(false, "Contraseña inválida.");

            query = "SELECT COUNT(1) FROM usuario WHERE upper(Username) = @username AND Id <> @id";
            var exist = await dbConnection.ExecuteScalarAsync<int>(query, new { username = updateUsuarioDTO.Username.ToUpper().Trim(), id = updateUsuarioDTO.Id });
            if (exist > 0)
                return new OperationResult<Usuario?>(false, "El nombre de usuario ya existe.");

            query = "UPDATE usuario SET Username = @username WHERE Id = @id";
            var result = await dbConnection.ExecuteAsync(query, new { username = updateUsuarioDTO.Username.Trim(), id = updateUsuarioDTO.Id });
            if (result == 0)
                return new OperationResult<Usuario?>(false, "No se pudo actualizar el usuario.");

            var updatedUsuario = new Usuario
            {
                Username = updateUsuarioDTO.Username.Trim(),
                Id = updateUsuarioDTO.Id
            };

            return new OperationResult<Usuario?>(true, "Usuario actualizado exitosamente.", updatedUsuario);
        }
        catch (Exception ex)
        {
            return new OperationResult<Usuario?>(false, "Error al actualizar el usuario.", exception: ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }
}
