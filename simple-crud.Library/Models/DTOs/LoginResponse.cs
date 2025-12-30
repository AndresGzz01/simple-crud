namespace simple_crud.Library.Models.DTOs;

public class LoginResponse
{
    public string Token { get; set; } = null!;

    public uint Id{ get; set; }

    public string Username { get; set; } = null!;
}
