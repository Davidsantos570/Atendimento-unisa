namespace Atendimento.Domain.Entities;

public class MensagemChamado
{
    public Guid Id { get; private set; }
    public string Autor { get; private set; } = null!;
    public string Texto { get; private set; } = null!;
    public DateTime EnviadaEm { get; private set; }

    private MensagemChamado() { }

    public MensagemChamado(string autor, string texto)
    {
        if (string.IsNullOrWhiteSpace(autor))
            throw new ArgumentException("Autor da mensagem é obrigatório.", nameof(autor));

        if (string.IsNullOrWhiteSpace(texto))
            throw new ArgumentException("Texto da mensagem é obrigatório.", nameof(texto));

        Id = Guid.NewGuid();
        Autor = autor;
        Texto = texto;
        EnviadaEm = DateTime.UtcNow;
    }
}
