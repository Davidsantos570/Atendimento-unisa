using System.Security.Claims;
using Atendimento.Application.Autenticacao;
using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atendimento.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/chamados")]
public class ChamadosController : ControllerBase
{
    private readonly IChamadoService _chamadoService;

    public ChamadosController(IChamadoService chamadoService)
    {
        _chamadoService = chamadoService;
    }

    [HttpPost]
    [Authorize(Roles = Papeis.Aluno)]
    public async Task<ActionResult<ChamadoDto>> Abrir([FromBody] AbrirChamadoDto dto)
    {
        var chamado = await _chamadoService.AbrirAsync(dto, ObterIdUsuarioAutenticado());
        return CreatedAtAction(nameof(ObterPorId), new { id = chamado.Id }, chamado);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChamadoDto>>> Listar()
    {
        if (EhConvidado())
        {
            var chamadoDoConvidado = await _chamadoService.ObterPorIdAsync(ObterChamadoIdDoConvidado());
            return Ok(chamadoDoConvidado is null ? Enumerable.Empty<ChamadoDto>() : [chamadoDoConvidado]);
        }

        var chamados = await _chamadoService.ListarAsync();
        return Ok(chamados);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ChamadoDto>> ObterPorId(Guid id)
    {
        if (EhConvidado() && ObterChamadoIdDoConvidado() != id)
            return Forbid();

        var chamado = await _chamadoService.ObterPorIdAsync(id);
        return chamado is null ? NotFound() : Ok(chamado);
    }

    [HttpPost("{id:guid}/iniciar-atendimento")]
    [Authorize(Roles = $"{Papeis.Atendente},{Papeis.Admin}")]
    public async Task<ActionResult<ChamadoDto>> IniciarAtendimento(Guid id, [FromBody] IniciarAtendimentoDto dto)
    {
        var chamado = await _chamadoService.IniciarAtendimentoAsync(id, dto.AtendenteId);
        return Ok(chamado);
    }

    [HttpPost("{id:guid}/concluir")]
    [Authorize(Roles = $"{Papeis.Atendente},{Papeis.Admin}")]
    public async Task<ActionResult<ChamadoDto>> Concluir(Guid id)
    {
        var chamado = await _chamadoService.ConcluirAsync(id, ObterIdUsuarioAutenticado());
        return Ok(chamado);
    }

    [HttpPost("{id:guid}/mensagens")]
    [Authorize(Roles = $"{Papeis.Aluno},{Papeis.Atendente},{Papeis.Admin}")]
    public async Task<ActionResult<ChamadoDto>> AdicionarMensagem(Guid id, [FromBody] AdicionarMensagemDto dto)
    {
        var autor = User.FindFirstValue(ClaimTypes.Name)!;
        var papel = User.FindFirstValue(ClaimTypes.Role)!;
        var chamado = await _chamadoService.AdicionarMensagemAsync(id, autor, dto.Texto, ObterIdUsuarioAutenticado(), papel);
        return Ok(chamado);
    }

    [HttpPost("{id:guid}/sugestao-ia")]
    [Authorize(Roles = $"{Papeis.Atendente},{Papeis.Admin}")]
    public async Task<ActionResult<SugestaoIaDto>> SugerirRespostaIa(Guid id)
    {
        var papel = User.FindFirstValue(ClaimTypes.Role)!;
        var sugestao = await _chamadoService.SugerirRespostaIaAsync(id, ObterIdUsuarioAutenticado(), papel);
        return Ok(new SugestaoIaDto(sugestao));
    }

    private Guid ObterIdUsuarioAutenticado() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool EhConvidado() => User.FindFirstValue(ClaimTypes.Role) == Papeis.Convidado;

    private Guid ObterChamadoIdDoConvidado() =>
        Guid.Parse(User.FindFirstValue(ClaimsPersonalizadas.ChamadoId)!);
}

public record IniciarAtendimentoDto(Guid AtendenteId);
