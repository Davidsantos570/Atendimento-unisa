using Atendimento.Domain.Entities;

namespace Atendimento.Domain.Repositories;

public interface IAdminRepository
{
    Task<Admin?> ObterPorEmailAsync(string email);
    Task<bool> ExisteAlgumAsync();
    Task AdicionarAsync(Admin admin);
}
