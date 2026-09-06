using Atendimento.Application.Auditoria;
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
    private readonly IAdminRepository _adminRepository;
    private readonly IHashDeSenha _hashDeSenha;
    private readonly IGeradorDeToken _geradorDeToken;
    private readonly IAuditoriaService _auditoriaService;

    public AutenticacaoService(
        IAlunoRepository alunoRepository,
        IAtendenteRepository atendenteRepository,
        IConvidadoRepository convidadoRepository,
        IAdminRepository adminRepository,
        IHashDeSenha hashDeSenha,
        IGeradorDeToken geradorDeToken,
        IAuditoriaService auditoriaService)
    {
        _alunoRepository = alunoRepository;
        _atendenteRepository = atendenteRepository;
        _convidadoRepository = convidadoRepository;
        _adminRepository = adminRepository;
        _hashDeSenha = hashDeSenha;
        _geradorDeToken = geradorDeToken;
        _auditoriaService = auditoriaService;
    }

    public async Task<TokenDto> LoginAsync(LoginDto dto)
    {
        var admin = await _adminRepository.ObterPorEmailAsync(dto.Email);
        if (admin is not null && _hashDeSenha.VerificarSenha(dto.Senha, admin.SenhaHash))
            return await GerarTokenComAuditoriaAsync(admin.Id, admin.Email, admin.Nome, Papeis.Admin, chamadoId: null);

        var aluno = await _alunoRepository.ObterPorEmailAsync(dto.Email);
        if (aluno is not null && _hashDeSenha.VerificarSenha(dto.Senha, aluno.SenhaHash))
            return await GerarTokenComAuditoriaAsync(aluno.Id, aluno.Email, aluno.Nome, Papeis.Aluno, chamadoId: null);

        var atendente = await _atendenteRepository.ObterPorEmailAsync(dto.Email);
        if (atendente is not null && _hashDeSenha.VerificarSenha(dto.Senha, atendente.SenhaHash))
            return await GerarTokenComAuditoriaAsync(atendente.Id, atendente.Email, atendente.Nome, Papeis.Atendente, chamadoId: null);

        var convidado = await _convidadoRepository.ObterPorEmailAsync(dto.Email);
        if (convidado is not null && _hashDeSenha.VerificarSenha(dto.Senha, convidado.SenhaHash))
            return await GerarTokenComAuditoriaAsync(convidado.Id, convidado.Email, convidado.Nome, Papeis.Convidado, convidado.ChamadoId);

        await _auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.Login,
            $"Falha no login para o email {dto.Email}: credenciais inválidas.",
            usuarioId: null,
            papel: null);

        throw new UnauthorizedAccessException("Email ou senha inválidos.");
    }

    private async Task<TokenDto> GerarTokenComAuditoriaAsync(Guid usuarioId, string email, string nome, string papel, Guid? chamadoId)
    {
        var (accessToken, expiraEm) = _geradorDeToken.Gerar(usuarioId, email, nome, papel, chamadoId);

        await _auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.Login,
            $"Login bem-sucedido para {email}.",
            usuarioId,
            papel);

        return new TokenDto(accessToken, expiraEm, papel, usuarioId, nome, chamadoId);
    }
}
