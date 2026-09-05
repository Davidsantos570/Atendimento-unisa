using Atendimento.Application.Autenticacao;
using Atendimento.Application.DTOs;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Repositories;

namespace Atendimento.Application.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IAtendenteRepository _atendenteRepository;
    private readonly IConvidadoRepository _convidadoRepository;
    private readonly IHashDeSenha _hashDeSenha;
    private readonly IGeradorDeToken _geradorDeToken;

    public AutenticacaoService(
        IAlunoRepository alunoRepository,
        IAtendenteRepository atendenteRepository,
        IConvidadoRepository convidadoRepository,
        IHashDeSenha hashDeSenha,
        IGeradorDeToken geradorDeToken)
    {
        _alunoRepository = alunoRepository;
        _atendenteRepository = atendenteRepository;
        _convidadoRepository = convidadoRepository;
        _hashDeSenha = hashDeSenha;
        _geradorDeToken = geradorDeToken;
    }

    public async Task<TokenDto> LoginAsync(LoginDto dto)
    {
        var aluno = await _alunoRepository.ObterPorEmailAsync(dto.Email);
        if (aluno is not null && _hashDeSenha.VerificarSenha(dto.Senha, aluno.SenhaHash))
            return GerarToken(aluno.Id, aluno.Email, aluno.Nome, Papeis.Aluno, chamadoId: null);

        var atendente = await _atendenteRepository.ObterPorEmailAsync(dto.Email);
        if (atendente is not null && _hashDeSenha.VerificarSenha(dto.Senha, atendente.SenhaHash))
            return GerarToken(atendente.Id, atendente.Email, atendente.Nome, Papeis.Atendente, chamadoId: null);

        var convidado = await _convidadoRepository.ObterPorEmailAsync(dto.Email);
        if (convidado is not null && _hashDeSenha.VerificarSenha(dto.Senha, convidado.SenhaHash))
            return GerarToken(convidado.Id, convidado.Email, convidado.Nome, Papeis.Convidado, convidado.ChamadoId);

        throw new UnauthorizedAccessException("Email ou senha inválidos.");
    }

    private TokenDto GerarToken(Guid usuarioId, string email, string nome, string papel, Guid? chamadoId)
    {
        var (accessToken, expiraEm) = _geradorDeToken.Gerar(usuarioId, email, nome, papel, chamadoId);
        return new TokenDto(accessToken, expiraEm, papel, usuarioId, nome, chamadoId);
    }
}
