using Atendimento.Domain.Entities;
using Atendimento.Domain.Repositories;
using Atendimento.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atendimento.Infrastructure.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly AtendimentoDbContext _dbContext;

    public AlunoRepository(AtendimentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Aluno?> ObterPorIdAsync(Guid id) => await _dbContext.Alunos.FindAsync(id);

    public async Task<Aluno?> ObterPorEmailAsync(string email) =>
        await _dbContext.Alunos.FirstOrDefaultAsync(aluno => aluno.Email == email);

    public async Task AdicionarAsync(Aluno aluno)
    {
        await _dbContext.Alunos.AddAsync(aluno);
        await _dbContext.SaveChangesAsync();
    }
}
