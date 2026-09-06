using Atendimento.Domain.Entities;

namespace Atendimento.Domain.Repositories;

public interface IConvidadoRepository
{
    Task<Convidado?> ObterPorIdAsync(Guid id);
    Task<Convidado?> ObterPorEmailAsync(string email);
    Task<IEnumerable<Convidado>> ListarAsync();
    Task AdicionarAsync(Convidado convidado);
}
