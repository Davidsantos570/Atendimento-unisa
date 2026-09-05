using Atendimento.Application.Interfaces;

namespace Atendimento.Infrastructure.Seguranca;

public class HashDeSenhaBCrypt : IHashDeSenha
{
    public string GerarHash(string senha) => BCrypt.Net.BCrypt.HashPassword(senha);

    public bool VerificarSenha(string senha, string senhaHash) => BCrypt.Net.BCrypt.Verify(senha, senhaHash);
}
