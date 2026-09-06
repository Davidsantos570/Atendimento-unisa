using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;
using Atendimento.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atendimento.Infrastructure.Repositories;

public class RegistroAuditoriaRepository : IRegistroAuditoriaRepository
{
    private readonly AtendimentoDbContext _dbContext;

    public RegistroAuditoriaRepository(AtendimentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(RegistroAuditoria registro)
    {
        await _dbContext.RegistrosAuditoria.AddAsync(registro);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<RegistroAuditoria>> ListarAsync() =>
        await _dbContext.RegistrosAuditoria
            .OrderByDescending(registro => registro.DataHoraUtc)
            .ToListAsync();
}
