using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atendimento.Api.Controllers;

[ApiController]
[Route("api/chamados")]
public class ChamadosController : ControllerBase
{
    private readonly IChamadoService _chamadoService;

    public ChamadosController(IChamadoService chamadoService)
    {
        _chamadoService = chamadoService;
    }

    [HttpPost]
    public async Task<ActionResult<ChamadoDto>> Abrir([FromBody] AbrirChamadoDto dto)
    {
        var chamado = await _chamadoService.AbrirAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = chamado.Id }, chamado);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChamadoDto>>> Listar()
    {
        var chamados = await _chamadoService.ListarAsync();
        return Ok(chamados);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ChamadoDto>> ObterPorId(Guid id)
    {
        var chamado = await _chamadoService.ObterPorIdAsync(id);
        return chamado is null ? NotFound() : Ok(chamado);
    }

    [HttpPost("{id:guid}/iniciar-atendimento")]
    public async Task<ActionResult<ChamadoDto>> IniciarAtendimento(Guid id, [FromBody] IniciarAtendimentoDto dto)
    {
        var chamado = await _chamadoService.IniciarAtendimentoAsync(id, dto.AtendenteId);
        return Ok(chamado);
    }

    [HttpPost("{id:guid}/concluir")]
    public async Task<ActionResult<ChamadoDto>> Concluir(Guid id)
    {
        var chamado = await _chamadoService.ConcluirAsync(id);
        return Ok(chamado);
    }

    [HttpPost("{id:guid}/responder-com-ia")]
    public async Task<ActionResult<ChamadoDto>> ResponderComIa(Guid id, [FromBody] ResponderComIaDto dto)
    {
        var chamado = await _chamadoService.ResponderComIaAsync(id, dto.MensagemAluno);
        return Ok(chamado);
    }
}

public record IniciarAtendimentoDto(Guid AtendenteId);

public record ResponderComIaDto(string MensagemAluno);
