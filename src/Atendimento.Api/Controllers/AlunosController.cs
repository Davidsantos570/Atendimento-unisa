using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atendimento.Api.Controllers;

[ApiController]
[Route("api/alunos")]
public class AlunosController : ControllerBase
{
    private readonly IAlunoService _alunoService;

    public AlunosController(IAlunoService alunoService)
    {
        _alunoService = alunoService;
    }

    [HttpPost]
    public async Task<ActionResult<AlunoDto>> Cadastrar([FromBody] CadastrarAlunoDto dto)
    {
        var aluno = await _alunoService.CadastrarAsync(dto);
        return CreatedAtAction(nameof(Cadastrar), new { id = aluno.Id }, aluno);
    }
}
