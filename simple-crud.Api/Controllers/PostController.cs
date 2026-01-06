using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using simple_crud.Api.Models;
using simple_crud.Library.Models.DTOs;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace simple_crud.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    readonly IValidator<PostDto> postValidator;
    readonly IDatabaseRepository databaseRepository;

    public PostController(IDatabaseRepository databaseRepository,
        IValidator<PostDto> postValidator)
    {
        this.databaseRepository = databaseRepository;
        this.postValidator = postValidator;
    }

    [HttpGet("/api/usuario/{usuarioId}/posts")]
    public async Task<IActionResult> GetPostsByUsuarioId(uint usuarioId)
    {
        var result = await databaseRepository.GetPostsByUsuarioId(usuarioId);

        if (!result.Success)
            return StatusCode(500, result.Message);

        return Ok(result.Value);
    }

    [HttpGet("/api/post/{titulo}")]
    public async Task<IActionResult> GetPostsByTitulo(string titulo)
    {
        var result = await databaseRepository.GetPostsByTitulo(titulo);

        if (!result.Success)
            return StatusCode(500, result.Message);

        return Ok(result.Value);
    }

    [HttpPost("/api/post")]
    public async Task<IActionResult> CreatePost([FromBody] PostDto post)
    {
        var validationResult = await postValidator.ValidateAsync(post);

        if (validationResult.IsValid is false)
        {
            var message = validationResult.Errors.First().ErrorMessage;
            return BadRequest(message);
        }

        var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(idUsuarioClaim)) 
            return Unauthorized("No se pudo obtener el Id del usuario del token.");

        if (!uint.TryParse(idUsuarioClaim, out uint idUsuario))
            return BadRequest();

        var model = new PostDto { Titulo = post.Titulo, Contenido = post.Contenido, IdUsuario = idUsuario };
        var result = await databaseRepository.CreatePost(model);

        if (result.Success is false)
            return BadRequest(result.Message);

        return Ok(new { result.Value!.Id, });
    }
}
