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

    /// <summary>
    /// Regresa un usuario si las credenciales son válidas.
    /// </summary>
    /// <param name="loginUsuarioDTO">Los detalles de inicio de sesión del usuario.</param>
    Task<OperationResult<Usuario?>> LoginUsuario(LoginUsuarioDTO loginUsuarioDTO);

    /// <summary>
    /// Se encarga de eliminar un usuario de la base de datos.
    /// </summary>
    /// <param name="deleteUsuarioDTO">Información necesaria para eliminar el usuario.</param>
    Task<OperationResult> DeleteUsuario(DeleteUsuarioDTO deleteUsuarioDTO);

    /// <summary>
    /// Actualiza la información de un usuario existente en la base de datos.
    /// </summary>
    /// <param name="updateUsuarioDTO">Información necesaria para actualizar el usuario.</param>
    Task<OperationResult<Usuario?>> UpdateUsuario(UpdateUsuarioDTO updateUsuarioDTO);

    /// <summary>
    /// Obtiene los posts asociados a un usuario específico por su ID.
    /// </summary>
    /// <param name="usuarioId">Indica el identificador único de cada usuario.</param>
    Task<OperationResult<IEnumerable<Post>>> GetPostsByUsuarioId(uint usuarioId);

    /// <summary>
    /// Obtiene el post que corresponde con el titulo proporcionado.
    /// </summary>
    /// <param name="titulo">Titulo por el que será identificado</param>
    Task<OperationResult<Post>> GetPostsByTitulo(string titulo);
}
