using Atendimento.Application.Auditoria;
using Atendimento.Application.Autenticacao;
using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Repositories;

namespace Atendimento.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IAtendenteService _atendenteService;
    private readonly IConvidadoService _convidadoService;
    private readonly IChamadoRepository _chamadoRepository;
    private readonly IAuditoriaService _auditoriaService;

    public UsuarioService(
        IAtendenteService atendenteService,
        IConvidadoService convidadoService,
        IChamadoRepository chamadoRepository,
        IAuditoriaService auditoriaService)
    {
        _atendenteService = atendenteService;
        _convidadoService = convidadoService;
        _chamadoRepository = chamadoRepository;
        _auditoriaService = auditoriaService;
    }

    public Task<UsuarioCriadoDto> CriarAsync(CriarUsuarioDto dto, SolicitanteDto? solicitante) => dto.Papel switch
    {
        Papeis.Atendente => CriarAtendenteAsync(dto),
        Papeis.Convidado => CriarConvidadoAsync(dto, solicitante),
        _ => throw new ArgumentException($"Papel '{dto.Papel}' inválido. Use '{Papeis.Atendente}' ou '{Papeis.Convidado}'.", nameof(dto))
    };

    private async Task<UsuarioCriadoDto> CriarAtendenteAsync(CriarUsuarioDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Setor))
            throw new ArgumentException("Setor é obrigatório para o papel Atendente.", nameof(dto));

        var atendente = await _atendenteService.CadastrarAsync(new CadastrarAtendenteDto(dto.Nome, dto.Email, dto.Setor, dto.Senha));

        await _auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.UsuarioCriado,
            $"Atendente {atendente.Email} cadastrado.",
            atendente.Id,
            Papeis.Atendente);

        return new UsuarioCriadoDto(atendente.Id, atendente.Nome, atendente.Email, Papeis.Atendente);
    }

    private async Task<UsuarioCriadoDto> CriarConvidadoAsync(CriarUsuarioDto dto, SolicitanteDto? solicitante)
    {
        if (dto.ChamadoId is null || dto.ChamadoId == Guid.Empty)
            throw new ArgumentException("ChamadoId é obrigatório para o papel Convidado.", nameof(dto));

        if (solicitante is null)
            throw new UnauthorizedAccessException("É preciso estar autenticado para convidar alguém para um chamado.");

        var chamado = await _chamadoRepository.ObterPorIdAsync(dto.ChamadoId.Value)
            ?? throw new InvalidOperationException($"Chamado {dto.ChamadoId} não encontrado.");

        var podeConvidar =
            (solicitante.Papel == Papeis.Aluno && chamado.AlunoId == solicitante.Id) ||
            (solicitante.Papel == Papeis.Atendente && chamado.AtendenteId == solicitante.Id);

        if (!podeConvidar)
            throw new UnauthorizedAccessException("Você não tem permissão para convidar alguém para este chamado.");

        var convidado = await _convidadoService.CadastrarAsync(new CadastrarConvidadoDto(dto.Nome, dto.Email, dto.Senha, dto.ChamadoId.Value));

        await _auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.UsuarioCriado,
            $"Convidado {convidado.Email} cadastrado para o chamado {convidado.ChamadoId} por {solicitante.Papel} {solicitante.Id}.",
            convidado.Id,
            Papeis.Convidado);

        return new UsuarioCriadoDto(convidado.Id, convidado.Nome, convidado.Email, Papeis.Convidado);
    }
}
