using System.Security.Claims;
using Atendimento.Application.Autenticacao;
using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atendimento.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioCriadoDto>> Criar([FromBody] CriarUsuarioDto dto)
    {
        var usuario = await _usuarioService.CriarAsync(dto, ObterSolicitanteAutenticado());
        return CreatedAtAction(nameof(Criar), new { id = usuario.Id }, usuario);
    }

    [HttpGet]
    [Authorize(Roles = Papeis.Admin)]
    public async Task<ActionResult<IEnumerable<UsuarioResumoDto>>> Listar()
    {
        var usuarios = await _usuarioService.ListarTodosAsync();
        return Ok(usuarios);
    }

    private SolicitanteDto? ObterSolicitanteAutenticado()
    {
        if (User.Identity?.IsAuthenticated != true)
            return null;

        return new SolicitanteDto(
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
            User.FindFirstValue(ClaimTypes.Role)!);
    }
}
