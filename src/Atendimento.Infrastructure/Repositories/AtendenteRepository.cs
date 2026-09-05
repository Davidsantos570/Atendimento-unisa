using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;
using Atendimento.Infrastructure.Persistence;

namespace Atendimento.Infrastructure.Repositories;

public class AtendenteRepository : IAtendenteRepository
{
    private readonly AtendimentoDbContext _dbContext;

    public AtendenteRepository(AtendimentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Atendente?> ObterPorIdAsync(Guid id) => await _dbContext.Atendentes.FindAsync(id);

    public async Task AdicionarAsync(Atendente atendente)
    {
        await _dbContext.Atendentes.AddAsync(atendente);
        await _dbContext.SaveChangesAsync();
    }
}
