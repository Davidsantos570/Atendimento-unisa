using Atendimento.Application.DTOs;

namespace Atendimento.Application.Interfaces;

public interface IChamadoService
{
    Task<ChamadoDto> AbrirAsync(AbrirChamadoDto dto, Guid alunoId);
    Task<ChamadoDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ChamadoDto>> ListarAsync();
    Task<ChamadoDto> IniciarAtendimentoAsync(Guid chamadoId, Guid atendenteId);
    Task<ChamadoDto> ConcluirAsync(Guid chamadoId, Guid atendenteId);
    Task<ChamadoDto> ResponderComIaAsync(Guid chamadoId, string mensagemAluno, Guid usuarioId, string papel);
}
