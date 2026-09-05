using Atendimento.Application.DTOs;

namespace Atendimento.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioCriadoDto> CriarAsync(CriarUsuarioDto dto, SolicitanteDto? solicitante);
}
