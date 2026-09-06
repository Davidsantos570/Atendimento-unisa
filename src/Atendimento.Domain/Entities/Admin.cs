namespace Atendimento.Domain.Entities;

public class Admin
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;

    private Admin() { }

    public Admin(string nome, string email, string senhaHash)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do admin é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email do admin é obrigatório.", nameof(email));

        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Senha do admin é obrigatória.", nameof(senhaHash));

        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
    }
}
