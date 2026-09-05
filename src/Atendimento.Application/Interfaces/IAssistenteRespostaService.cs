using Atendimento.Domain.Entities;

namespace Atendimento.Application.Interfaces;

public interface IAssistenteRespostaService
{
    Task<string> GerarRespostaAsync(Chamado chamado, string mensagemRecebida);
}
