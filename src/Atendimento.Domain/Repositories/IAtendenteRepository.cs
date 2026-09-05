using Atendimento.Domain.Entities;

namespace Atendimento.Domain.Repositories;

public interface IAtendenteRepository
{
    Task<Atendente?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Atendente atendente);
}
