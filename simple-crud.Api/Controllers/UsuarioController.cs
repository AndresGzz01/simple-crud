using Microsoft.AspNetCore.Mvc;

using simple_crud.Api.Models;

namespace simple_crud.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IDatabaseRepository databaseRepository;

    public UsuarioController(IDatabaseRepository databaseRepository)
    {
        this.databaseRepository = databaseRepository;
    }

    [HttpGet("{id:required}")]
    public async Task<IActionResult> GetUsuarioById(uint id)
    {
        var operationUsuario = await databaseRepository.GetUsuarioById(id);

        if (!operationUsuario.Success)
            return Problem(detail: operationUsuario.Message);

        var usuario = operationUsuario.Value!;
        return Ok(new { usuario.Id, usuario.Username });
    }
}
