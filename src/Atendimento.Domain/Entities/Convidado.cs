namespace Atendimento.Domain.Entities;

public class Convidado
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    public Guid ChamadoId { get; private set; }

    private Convidado() { }

    public Convidado(string nome, string email, string senhaHash, Guid chamadoId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do convidado é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email do convidado é obrigatório.", nameof(email));

        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Senha do convidado é obrigatória.", nameof(senhaHash));

        if (chamadoId == Guid.Empty)
            throw new ArgumentException("Convidado deve estar vinculado a um chamado.", nameof(chamadoId));

        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        ChamadoId = chamadoId;
    }
}
