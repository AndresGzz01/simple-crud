namespace simple_crud.Library.Models.DTOs;

public class UpdateUsuarioDTO
{
    public uint Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
