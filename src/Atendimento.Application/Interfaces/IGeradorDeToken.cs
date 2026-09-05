namespace Atendimento.Application.Interfaces;

public interface IGeradorDeToken
{
    (string AccessToken, DateTime ExpiraEm) Gerar(Guid usuarioId, string email, string nome, string papel);
}
