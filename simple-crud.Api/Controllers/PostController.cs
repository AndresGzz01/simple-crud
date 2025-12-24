using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using simple_crud.Api.Models;

namespace simple_crud.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    readonly IDatabaseRepository databaseRepository;

    public PostController(IDatabaseRepository databaseRepository)
    {
        this.databaseRepository = databaseRepository;
    }

    [AllowAnonymous]
    [HttpGet("/usuarios/{usuarioId}/posts")]
    public async Task<IActionResult> GetPostsByUsuarioId(uint usuarioId)
    {
        var result = await databaseRepository.GetPostsByUsuarioId(usuarioId);

        if (!result.Success)
            return StatusCode(500, result.Message);
        
        return Ok(result.Value);
    }
}
