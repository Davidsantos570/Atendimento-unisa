using Atendimento.Application.DTOs;

namespace Atendimento.Application.Interfaces;

public interface IAtendenteService
{
    Task<AtendenteDto> CadastrarAsync(CadastrarAtendenteDto dto);
}
