using simple_crud.Library.Models;
using simple_crud.Library.Models.DTOs;

namespace simple_crud.Client.Models;

public interface IBlogService
{
    Task<OperationResult> RegisterAsync(CreateUsuarioDTO createUsuarioDTO);

    Task<OperationResult> LoginAsync(LoginUsuarioDTO loginUsuarioDTO);

    Task<OperationResult> LogoutAsync();

    Task<OperationResult<IEnumerable<PostDto>>> GetPostByUserAsync(uint idUser);

    Task<OperationResult<PostDto?>> GetPostByTitle(string title);
}