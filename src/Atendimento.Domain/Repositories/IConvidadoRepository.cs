using Atendimento.Domain.Entities;

namespace Atendimento.Domain.Repositories;

public interface IConvidadoRepository
{
    Task<Convidado?> ObterPorIdAsync(Guid id);
    Task<Convidado?> ObterPorEmailAsync(string email);
    Task AdicionarAsync(Convidado convidado);
}
