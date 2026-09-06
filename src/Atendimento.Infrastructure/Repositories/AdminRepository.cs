using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;
using Atendimento.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atendimento.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly AtendimentoDbContext _dbContext;

    public AdminRepository(AtendimentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Admin?> ObterPorEmailAsync(string email) =>
        await _dbContext.Admins.FirstOrDefaultAsync(admin => admin.Email == email);

    public async Task<bool> ExisteAlgumAsync() => await _dbContext.Admins.AnyAsync();

    public async Task AdicionarAsync(Admin admin)
    {
        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();
    }
}
