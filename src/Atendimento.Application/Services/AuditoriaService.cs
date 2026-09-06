using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;

namespace Atendimento.Application.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IRegistroAuditoriaRepository _registroAuditoriaRepository;

    public AuditoriaService(IRegistroAuditoriaRepository registroAuditoriaRepository)
    {
        _registroAuditoriaRepository = registroAuditoriaRepository;
    }

    public async Task RegistrarAsync(
        string tipoEvento,
        string descricao,
        Guid? usuarioId = null,
        string? papel = null,
        string? metodoHttp = null,
        string? caminhoRequisicao = null,
        int? codigoStatus = null)
    {
        var registro = new RegistroAuditoria(tipoEvento, descricao, usuarioId, papel, metodoHttp, caminhoRequisicao, codigoStatus);
        await _registroAuditoriaRepository.AdicionarAsync(registro);
    }

    public async Task<IEnumerable<RegistroAuditoriaDto>> ListarAsync()
    {
        var registros = await _registroAuditoriaRepository.ListarAsync();
        return registros.Select(registro => new RegistroAuditoriaDto(
            registro.Id,
            registro.DataHoraUtc,
            registro.TipoEvento,
            registro.Descricao,
            registro.UsuarioId,
            registro.Papel,
            registro.MetodoHttp,
            registro.CaminhoRequisicao,
            registro.CodigoStatus));
    }
}
