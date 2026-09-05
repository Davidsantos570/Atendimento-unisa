using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;

namespace Atendimento.Application.Services;

public class AtendenteService : IAtendenteService
{
    private readonly IAtendenteRepository _atendenteRepository;
    private readonly IHashDeSenha _hashDeSenha;

    public AtendenteService(IAtendenteRepository atendenteRepository, IHashDeSenha hashDeSenha)
    {
        _atendenteRepository = atendenteRepository;
        _hashDeSenha = hashDeSenha;
    }

    public async Task<AtendenteDto> CadastrarAsync(CadastrarAtendenteDto dto)
    {
        var existente = await _atendenteRepository.ObterPorEmailAsync(dto.Email);
        if (existente is not null)
            throw new InvalidOperationException($"Já existe um atendente cadastrado com o email {dto.Email}.");

        var senhaHash = _hashDeSenha.GerarHash(dto.Senha);
        var atendente = new Atendente(dto.Nome, dto.Email, dto.Setor, senhaHash);

        await _atendenteRepository.AdicionarAsync(atendente);

        return new AtendenteDto(atendente.Id, atendente.Nome, atendente.Email, atendente.Setor);
    }
}
