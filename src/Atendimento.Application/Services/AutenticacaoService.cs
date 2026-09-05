using Atendimento.Application.Autenticacao;
using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Repositories;

namespace Atendimento.Application.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IAtendenteRepository _atendenteRepository;
    private readonly IHashDeSenha _hashDeSenha;
    private readonly IGeradorDeToken _geradorDeToken;

    public AutenticacaoService(
        IAlunoRepository alunoRepository,
        IAtendenteRepository atendenteRepository,
        IHashDeSenha hashDeSenha,
        IGeradorDeToken geradorDeToken)
    {
        _alunoRepository = alunoRepository;
        _atendenteRepository = atendenteRepository;
        _hashDeSenha = hashDeSenha;
        _geradorDeToken = geradorDeToken;
    }

    public async Task<TokenDto> LoginAsync(LoginDto dto)
    {
        var aluno = await _alunoRepository.ObterPorEmailAsync(dto.Email);
        if (aluno is not null && _hashDeSenha.VerificarSenha(dto.Senha, aluno.SenhaHash))
            return GerarToken(aluno.Id, aluno.Email, aluno.Nome, Papeis.Aluno);

        var atendente = await _atendenteRepository.ObterPorEmailAsync(dto.Email);
        if (atendente is not null && _hashDeSenha.VerificarSenha(dto.Senha, atendente.SenhaHash))
            return GerarToken(atendente.Id, atendente.Email, atendente.Nome, Papeis.Atendente);

        throw new UnauthorizedAccessException("Email ou senha inválidos.");
    }

    private TokenDto GerarToken(Guid usuarioId, string email, string nome, string papel)
    {
        var (accessToken, expiraEm) = _geradorDeToken.Gerar(usuarioId, email, nome, papel);
        return new TokenDto(accessToken, expiraEm, papel, usuarioId, nome);
    }
}
