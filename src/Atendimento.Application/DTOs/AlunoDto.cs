namespace Atendimento.Application.DTOs;

public record AlunoDto(Guid Id, string Nome, string Email, string Matricula);

public record CadastrarAlunoDto(string Nome, string Email, string Matricula, string Senha);
