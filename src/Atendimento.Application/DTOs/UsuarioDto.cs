namespace Atendimento.Application.DTOs;

public record CriarUsuarioDto(string Papel, string Nome, string Email, string Senha, string? Setor, Guid? ChamadoId);

public record UsuarioCriadoDto(Guid Id, string Nome, string Email, string Papel);

public record SolicitanteDto(Guid Id, string Papel);
