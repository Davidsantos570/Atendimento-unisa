using Atendimento.Application.DTOs;

namespace Atendimento.Application.Interfaces;

public interface IChamadoService
{
    Task<ChamadoDto> AbrirAsync(AbrirChamadoDto dto, Guid alunoId);
    Task<ChamadoDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ChamadoDto>> ListarAsync();
    Task<ChamadoDto> IniciarAtendimentoAsync(Guid chamadoId, Guid atendenteId);
    Task<ChamadoDto> ConcluirAsync(Guid chamadoId, Guid atendenteId);
    Task<ChamadoDto> AdicionarMensagemAsync(Guid chamadoId, string autor, string texto, Guid usuarioId, string papel);
    Task<string> SugerirRespostaIaAsync(Guid chamadoId, Guid usuarioId, string papel);
}
