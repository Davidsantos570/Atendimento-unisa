namespace Atendimento.Domain.Entities;

public class Atendente
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Setor { get; private set; } = null!;

    private Atendente() { }

    public Atendente(string nome, string email, string setor)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do atendente é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email do atendente é obrigatório.", nameof(email));

        if (string.IsNullOrWhiteSpace(setor))
            throw new ArgumentException("Setor do atendente é obrigatório.", nameof(setor));

        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Setor = setor;
    }
}
