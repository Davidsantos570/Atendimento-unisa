using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;
using Atendimento.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atendimento.Infrastructure.Repositories;

public class ChamadoRepository : IChamadoRepository
{
    private readonly AtendimentoDbContext _dbContext;

    public ChamadoRepository(AtendimentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Chamado?> ObterPorIdAsync(Guid id) =>
        await _dbContext.Chamados
            .Include(c => c.Mensagens)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Chamado>> ListarAsync() =>
        await _dbContext.Chamados
            .Include(c => c.Mensagens)
            .ToListAsync();

    public async Task AdicionarAsync(Chamado chamado)
    {
        await _dbContext.Chamados.AddAsync(chamado);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Chamado chamado)
    {
        await _dbContext.SaveChangesAsync();
    }
}
