namespace Atendimento.Domain.Entities;

public class Aluno
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Matricula { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;

    private Aluno() { }

    public Aluno(string nome, string email, string matricula, string senhaHash)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do aluno é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email do aluno é obrigatório.", nameof(email));

        if (string.IsNullOrWhiteSpace(matricula))
            throw new ArgumentException("Matrícula do aluno é obrigatória.", nameof(matricula));

        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Senha do aluno é obrigatória.", nameof(senhaHash));

        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Matricula = matricula;
        SenhaHash = senhaHash;
    }
}
