namespace simple_crud.Api.Models;

/// <summary>
/// Representa a una cuenta de usuario con información de identificación y autenticación.
/// </summary>
public class Usuario
{
    /// <summary>
    /// Obtiene el identificador único para esta instancia.
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Obtiene el nombre de usuario asociado con la instancia actual.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Obtiene la contraseña asociada con la instancia actual.
    /// </summary>
    public string? Password { get; set; }
}
