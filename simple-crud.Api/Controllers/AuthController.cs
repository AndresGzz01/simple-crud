using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using simple_crud.Api.Models;
using simple_crud.Library.Models.DTOs;

using System.Security.Claims;

namespace simple_crud.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IDatabaseRepository databaseRepository;

    public AuthController(IDatabaseRepository databaseRepository)
    {
        this.databaseRepository = databaseRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioDTO createUsuarioDTO)
    {
        var operationCreate = await databaseRepository.CreateUsuario(createUsuarioDTO);

        if (!operationCreate.Success)
            return Problem(detail: operationCreate.Message);

        var createdUsuario = operationCreate.Value!;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, createdUsuario.Id.ToString()),
            new(ClaimTypes.Name, createdUsuario.Username)
        };

        await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "login")));

        return Ok(new { createdUsuario.Id, createdUsuario.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDTO loginUsuarioDTO)
    {
        var operationLogin = await databaseRepository.LoginUsuario(loginUsuarioDTO);
        if (!operationLogin.Success)
            return Problem(detail: operationLogin.Message);
        var loggedInUsuario = operationLogin.Value!;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, loggedInUsuario.Id.ToString()),
            new(ClaimTypes.Name, loggedInUsuario.Username)
        };

        await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "login")));

        return Ok(new { loggedInUsuario.Id, loggedInUsuario.Username });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}
