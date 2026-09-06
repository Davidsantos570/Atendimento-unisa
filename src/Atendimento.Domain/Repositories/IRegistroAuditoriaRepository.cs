using Atendimento.Domain.Entities;

namespace Atendimento.Domain.Repositories;

public interface IRegistroAuditoriaRepository
{
    Task AdicionarAsync(RegistroAuditoria registro);
    Task<IEnumerable<RegistroAuditoria>> ListarAsync();
}
