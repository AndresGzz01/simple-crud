using simple_crud.Library.Models;
using simple_crud.Library.Models.DTOs;

namespace simple_crud.Api.Models;

/// <summary>
/// Define un contrato para acceder y recuperar entidades de usuario desde un repositorio de base de datos.
/// </summary>
public interface IDatabaseRepository
{
    /// <summary>
    /// Regresa un usuario por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del usuario a recuperar. Debe ser mayor que cero.</param>
    Task<OperationResult<Usuario?>> GetUsuarioById(uint id);

    /// <summary>
    /// Asíncronamente obtiene una lista de entidades de usuario.
    /// </summary>
    Task<OperationResult<IEnumerable<Usuario>>> GetUsuarios();

    /// <summary>
    /// Crea un nuevo usuario usando el objeto de transferencia de datos especificado.
    /// </summary>
    /// <param name="createUsuarioDTO">Un objeto que contiene la información requerida para crear el usuario. No puede ser nulo.</param>
    Task<OperationResult<Usuario>> CreateUsuario(CreateUsuarioDTO createUsuarioDTO);
}
