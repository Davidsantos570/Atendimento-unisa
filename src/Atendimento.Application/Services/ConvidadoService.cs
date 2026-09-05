using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;

namespace Atendimento.Application.Services;

public class ConvidadoService : IConvidadoService
{
    private readonly IConvidadoRepository _convidadoRepository;
    private readonly IChamadoRepository _chamadoRepository;
    private readonly IHashDeSenha _hashDeSenha;

    public ConvidadoService(
        IConvidadoRepository convidadoRepository,
        IChamadoRepository chamadoRepository,
        IHashDeSenha hashDeSenha)
    {
        _convidadoRepository = convidadoRepository;
        _chamadoRepository = chamadoRepository;
        _hashDeSenha = hashDeSenha;
    }

    public async Task<ConvidadoDto> CadastrarAsync(CadastrarConvidadoDto dto)
    {
        var chamado = await _chamadoRepository.ObterPorIdAsync(dto.ChamadoId)
            ?? throw new InvalidOperationException($"Chamado {dto.ChamadoId} não encontrado.");

        var existente = await _convidadoRepository.ObterPorEmailAsync(dto.Email);
        if (existente is not null)
            throw new InvalidOperationException($"Já existe um convidado cadastrado com o email {dto.Email}.");

        var senhaHash = _hashDeSenha.GerarHash(dto.Senha);
        var convidado = new Convidado(dto.Nome, dto.Email, senhaHash, chamado.Id);

        await _convidadoRepository.AdicionarAsync(convidado);

        return new ConvidadoDto(convidado.Id, convidado.Nome, convidado.Email, convidado.ChamadoId);
    }
}
