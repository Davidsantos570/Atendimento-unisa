using Atendimento.Application.Auditoria;
using Atendimento.Application.Autenticacao;
using Atendimento.Application.DTOs;
using Atendimento.Domain.Entities;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Repositories;

namespace Atendimento.Application.Services;

public class ChamadoService : IChamadoService
{
    private const string AutorAssistenteVirtual = "Assistente Virtual";

    private readonly IChamadoRepository _chamadoRepository;
    private readonly IAssistenteRespostaService _assistenteRespostaService;
    private readonly IAuditoriaService _auditoriaService;

    public ChamadoService(
        IChamadoRepository chamadoRepository,
        IAssistenteRespostaService assistenteRespostaService,
        IAuditoriaService auditoriaService)
    {
        _chamadoRepository = chamadoRepository;
        _assistenteRespostaService = assistenteRespostaService;
        _auditoriaService = auditoriaService;
    }

    public async Task<ChamadoDto> AbrirAsync(AbrirChamadoDto dto, Guid alunoId)
    {
        var chamado = new Chamado(dto.Titulo, dto.Descricao, alunoId);
        await _chamadoRepository.AdicionarAsync(chamado);

        await _auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.ChamadoAberto,
            $"Chamado {chamado.Id} aberto: \"{chamado.Titulo}\".",
            alunoId,
            Papeis.Aluno);

        return Mapear(chamado);
    }

    public async Task<ChamadoDto?> ObterPorIdAsync(Guid id)
    {
        var chamado = await _chamadoRepository.ObterPorIdAsync(id);
        return chamado is null ? null : Mapear(chamado);
    }

    public async Task<IEnumerable<ChamadoDto>> ListarAsync()
    {
        var chamados = await _chamadoRepository.ListarAsync();
        return chamados.Select(Mapear);
    }

    public async Task<ChamadoDto> IniciarAtendimentoAsync(Guid chamadoId, Guid atendenteId)
    {
        var chamado = await ObterOuFalharAsync(chamadoId);
        chamado.IniciarAtendimento(atendenteId);
        await _chamadoRepository.AtualizarAsync(chamado);

        await _auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.ChamadoIniciado,
            $"Chamado {chamado.Id} teve atendimento iniciado.",
            atendenteId,
            Papeis.Atendente);

        return Mapear(chamado);
    }

    public async Task<ChamadoDto> ConcluirAsync(Guid chamadoId, Guid atendenteId)
    {
        var chamado = await ObterOuFalharAsync(chamadoId);
        chamado.Concluir();
        await _chamadoRepository.AtualizarAsync(chamado);

        await _auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.ChamadoConcluido,
            $"Chamado {chamado.Id} concluído.",
            atendenteId,
            Papeis.Atendente);

        return Mapear(chamado);
    }

    public async Task<ChamadoDto> ResponderComIaAsync(Guid chamadoId, string mensagemAluno, Guid usuarioId, string papel)
    {
        var chamado = await ObterOuFalharAsync(chamadoId);

        chamado.AdicionarMensagem(nameof(Aluno), mensagemAluno);

        var resposta = await _assistenteRespostaService.GerarRespostaAsync(chamado, mensagemAluno);
        chamado.AdicionarMensagem(AutorAssistenteVirtual, resposta);

        await _chamadoRepository.AtualizarAsync(chamado);

        await _auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.ChamadoRespostaIa,
            $"IA respondeu no chamado {chamado.Id}.",
            usuarioId,
            papel);

        return Mapear(chamado);
    }

    private async Task<Chamado> ObterOuFalharAsync(Guid chamadoId)
    {
        var chamado = await _chamadoRepository.ObterPorIdAsync(chamadoId);
        if (chamado is null)
            throw new InvalidOperationException($"Chamado {chamadoId} não encontrado.");

        return chamado;
    }

    private static ChamadoDto Mapear(Chamado chamado) => new(
        chamado.Id,
        chamado.Titulo,
        chamado.Descricao,
        chamado.Status,
        chamado.AlunoId,
        chamado.AtendenteId,
        chamado.CriadoEm,
        chamado.ConcluidoEm,
        chamado.Mensagens.Select(m => new MensagemDto(m.Autor, m.Texto, m.EnviadaEm)).ToList());
}
