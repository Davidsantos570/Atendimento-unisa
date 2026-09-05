using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;

namespace Atendimento.Application.Services;

public class AlunoService : IAlunoService
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IHashDeSenha _hashDeSenha;

    public AlunoService(IAlunoRepository alunoRepository, IHashDeSenha hashDeSenha)
    {
        _alunoRepository = alunoRepository;
        _hashDeSenha = hashDeSenha;
    }

    public async Task<AlunoDto> CadastrarAsync(CadastrarAlunoDto dto)
    {
        var existente = await _alunoRepository.ObterPorEmailAsync(dto.Email);
        if (existente is not null)
            throw new InvalidOperationException($"Já existe um aluno cadastrado com o email {dto.Email}.");

        var senhaHash = _hashDeSenha.GerarHash(dto.Senha);
        var aluno = new Aluno(dto.Nome, dto.Email, dto.Matricula, senhaHash);

        await _alunoRepository.AdicionarAsync(aluno);

        return new AlunoDto(aluno.Id, aluno.Nome, aluno.Email, aluno.Matricula);
    }
}
