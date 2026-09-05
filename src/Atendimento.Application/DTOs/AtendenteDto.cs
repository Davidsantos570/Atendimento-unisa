namespace Atendimento.Application.DTOs;

public record AtendenteDto(Guid Id, string Nome, string Email, string Setor);

public record CadastrarAtendenteDto(string Nome, string Email, string Setor, string Senha);
