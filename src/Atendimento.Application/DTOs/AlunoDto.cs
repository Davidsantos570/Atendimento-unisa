namespace Atendimento.Application.DTOs;

public record AlunoDto(Guid Id, string Nome, string Email, string Matricula, string Telefone, string Curso);

public record CadastrarAlunoDto(string Nome, string Email, string Matricula, string Telefone, string Curso, string Senha);
