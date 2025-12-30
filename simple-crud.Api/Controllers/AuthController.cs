using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using simple_crud.Api.Models;
using simple_crud.Library.Models.DTOs;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace simple_crud.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly IDatabaseRepository databaseRepository;
    readonly IConfiguration configuration;

    public AuthController(IDatabaseRepository databaseRepository, IConfiguration configuration)
    {
        this.databaseRepository = databaseRepository;
        this.configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioDTO createUsuarioDTO)
    {
        var operationCreate = await databaseRepository.CreateUsuario(createUsuarioDTO);

        if (!operationCreate.Success)
            return Problem(detail: operationCreate.Message);

        var createdUsuario = operationCreate.Value!;

        var secretKey = configuration.GetValue<string>("SecretKey");

        if (string.IsNullOrEmpty(secretKey))
            return Problem(detail: "Secret key is not configured.");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, createdUsuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, createdUsuario.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken( issuer: "simple-crud.api", audience: "simple-crud.client", claims: claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), createdUsuario.Id, createdUsuario.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDTO loginUsuarioDTO)
    {
        var operationLogin = await databaseRepository.LoginUsuario(loginUsuarioDTO);
        if (!operationLogin.Success)
            return Problem(detail: operationLogin.Message);
        var loggedInUsuario = operationLogin.Value!;

        var secretKey = configuration.GetValue<string>("SecretKey");

        if (string.IsNullOrEmpty(secretKey))
            return Problem(detail: "Secret key is not configured.");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, loggedInUsuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, loggedInUsuario.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); 
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 
        var token = new JwtSecurityToken( issuer: "simple-crud.api", audience: "simple-crud.client", claims: claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds); 
        
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), loggedInUsuario.Id, loggedInUsuario.Username });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}
