namespace simple_crud.Api.Models;

public interface IPasswordHasher
{
    string HashPassword(string password);
}
