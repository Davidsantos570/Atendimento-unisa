namespace Atendimento.Application.Interfaces;

public interface IHashDeSenha
{
    string GerarHash(string senha);
    bool VerificarSenha(string senha, string senhaHash);
}
