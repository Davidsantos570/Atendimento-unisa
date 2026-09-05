namespace Atendimento.Application.DTOs;

public record ConvidadoDto(Guid Id, string Nome, string Email, Guid ChamadoId);

public record CadastrarConvidadoDto(string Nome, string Email, string Senha, Guid ChamadoId);
