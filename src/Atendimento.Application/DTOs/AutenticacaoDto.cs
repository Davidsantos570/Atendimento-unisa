namespace Atendimento.Application.DTOs;

public record LoginDto(string Email, string Senha);

public record TokenDto(string AccessToken, DateTime ExpiraEm, string Papel, Guid UsuarioId, string Nome, Guid? ChamadoId);
