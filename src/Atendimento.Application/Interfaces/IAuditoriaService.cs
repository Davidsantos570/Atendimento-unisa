using Atendimento.Application.DTOs;

namespace Atendimento.Application.Interfaces;

public interface IAuditoriaService
{
    Task RegistrarAsync(
        string tipoEvento,
        string descricao,
        Guid? usuarioId = null,
        string? papel = null,
        string? metodoHttp = null,
        string? caminhoRequisicao = null,
        int? codigoStatus = null);

    Task<IEnumerable<RegistroAuditoriaDto>> ListarAsync();
}
