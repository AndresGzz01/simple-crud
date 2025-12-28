using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using simple_crud.Api.Models;
using simple_crud.Library.Models.DTOs;

namespace simple_crud.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IDatabaseRepository databaseRepository;

    public UsuarioController(IDatabaseRepository databaseRepository)
    {
        this.databaseRepository = databaseRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        var operationUsuarios = await databaseRepository.GetUsuarios();
     
        if (!operationUsuarios.Success)
            return Problem(detail: operationUsuarios.Message);
        
        var usuarios = operationUsuarios.Value!;
        var result = usuarios.Select(usuario => new { usuario.Id, usuario.Username });
        
        return Ok(result);
    }

    [HttpGet("{id:required}")]
    public async Task<IActionResult> GetUsuarioById(uint id)
    {
        var operationUsuario = await databaseRepository.GetUsuarioById(id);

        if (!operationUsuario.Success)
            return Problem(detail: operationUsuario.Message);

        if (operationUsuario.Value is null)
            return NotFound();

        var usuario = operationUsuario.Value!;
        return Ok(new { usuario.Id, usuario.Username });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUsuario([FromBody] DeleteUsuarioDTO deleteUsuarioDTO)
    {
        var operationDelete = await databaseRepository.DeleteUsuario(deleteUsuarioDTO);

        if (!operationDelete.Success)
            return Problem(detail: operationDelete.Message);
        
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUsuario([FromBody] UpdateUsuarioDTO updateUsuarioDTO)
    {
        var operationUpdate = await databaseRepository.UpdateUsuario(updateUsuarioDTO);
        if (!operationUpdate.Success)
            return Problem(detail: operationUpdate.Message);

        var updatedUsuario = operationUpdate.Value!;
        return Ok(new { updatedUsuario.Id, updatedUsuario.Username });
    }
}
