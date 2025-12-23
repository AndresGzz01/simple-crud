namespace simple_crud.Api.Models;

/// <summary>
/// Define un contrato para el hashing y verificación de contraseñas.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Se encarga de hashear la contraseña proporcionada.
    /// </summary>
    /// <param name="password">Es la contraseña en texto plano que se desea hashear.</param>
    /// <returns>Es el hash resultante de la contraseña proporcionada.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Se encarga de verificar si la contraseña proporcionada coincide con el hash almacenado.
    /// </summary>
    /// <param name="hashedPassword">Es el hash almacenado de la contraseña.</param>
    /// <param name="providedPassword">Es la contraseña en texto plano que se desea verificar.</param>
    /// <returns>Devuelve true si la contraseña proporcionada coincide con el hash almacenado; de lo contrario, false.</returns>
    bool VerifyPassword(string hashedPassword, string providedPassword);
}
