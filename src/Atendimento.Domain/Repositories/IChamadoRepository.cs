using Atendimento.Domain.Entities;

namespace Atendimento.Domain.Repositories;

public interface IChamadoRepository
{
    Task<Chamado?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Chamado>> ListarAsync();
    Task AdicionarAsync(Chamado chamado);
    Task AtualizarAsync(Chamado chamado);
}
