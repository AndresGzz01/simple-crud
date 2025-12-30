namespace simple_crud.Library.Models.DTOs;

/// <summary>
/// Representa un post o artículo de blog.
/// </summary>
public class PostDto
{
    /// <summary>
    /// Obtiene el identificador único para esta instancia.
    /// </summary>
    public uint Id { get; init; }

    /// <summary>
    /// Obtiene el título del post.
    /// </summary>
    public string Titulo { get; init; } = null!;

    /// <summary>
    /// Obtiene la fecha y hora de la última actualización del post.
    /// </summary>
    public DateTime UltimaActualizacion { get; init; }

    /// <summary>
    /// Obtiene el contenido del post (Markdown).
    /// </summary>
    public string? Contenido { get; init; }

    /// <summary>
    /// Obtiene el identificador del usuario que creó el post.
    /// </summary>
    public uint IdUsuario { get; init; }
}
