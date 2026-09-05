using Atendimento.Application.DTOs;

namespace Atendimento.Application.Interfaces;

public interface IAlunoService
{
    Task<AlunoDto> CadastrarAsync(CadastrarAlunoDto dto);
}
