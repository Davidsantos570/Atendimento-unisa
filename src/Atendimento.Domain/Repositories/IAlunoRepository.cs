using Atendimento.Domain.Entities;

namespace Atendimento.Domain.Repositories;

public interface IAlunoRepository
{
    Task<Aluno?> ObterPorIdAsync(Guid id);
    Task<Aluno?> ObterPorEmailAsync(string email);
    Task AdicionarAsync(Aluno aluno);
}
