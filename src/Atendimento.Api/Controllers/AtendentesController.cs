using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atendimento.Api.Controllers;

[ApiController]
[Route("api/atendentes")]
public class AtendentesController : ControllerBase
{
    private readonly IAtendenteService _atendenteService;

    public AtendentesController(IAtendenteService atendenteService)
    {
        _atendenteService = atendenteService;
    }

    [HttpPost]
    public async Task<ActionResult<AtendenteDto>> Cadastrar([FromBody] CadastrarAtendenteDto dto)
    {
        var atendente = await _atendenteService.CadastrarAsync(dto);
        return CreatedAtAction(nameof(Cadastrar), new { id = atendente.Id }, atendente);
    }
}
