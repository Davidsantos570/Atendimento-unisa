using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;
using Atendimento.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atendimento.Infrastructure.Repositories;

public class ConvidadoRepository : IConvidadoRepository
{
    private readonly AtendimentoDbContext _dbContext;

    public ConvidadoRepository(AtendimentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Convidado?> ObterPorIdAsync(Guid id) => await _dbContext.Convidados.FindAsync(id);

    public async Task<Convidado?> ObterPorEmailAsync(string email) =>
        await _dbContext.Convidados.FirstOrDefaultAsync(convidado => convidado.Email == email);

    public async Task AdicionarAsync(Convidado convidado)
    {
        await _dbContext.Convidados.AddAsync(convidado);
        await _dbContext.SaveChangesAsync();
    }
}
