using Atendimento.Application.DTOs;

namespace Atendimento.Application.Interfaces;

public interface IConvidadoService
{
    Task<ConvidadoDto> CadastrarAsync(CadastrarConvidadoDto dto);
}
