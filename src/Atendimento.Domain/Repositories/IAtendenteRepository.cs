using Atendimento.Domain.Entities;

namespace Atendimento.Domain.Repositories;

public interface IAtendenteRepository
{
    Task<Atendente?> ObterPorIdAsync(Guid id);
    Task<Atendente?> ObterPorEmailAsync(string email);
    Task AdicionarAsync(Atendente atendente);
}
