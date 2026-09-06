namespace Atendimento.Domain.Entities;

public class RegistroAuditoria
{
    public Guid Id { get; private set; }
    public DateTime DataHoraUtc { get; private set; }
    public string TipoEvento { get; private set; } = null!;
    public string Descricao { get; private set; } = null!;
    public Guid? UsuarioId { get; private set; }
    public string? Papel { get; private set; }
    public string? MetodoHttp { get; private set; }
    public string? CaminhoRequisicao { get; private set; }
    public int? CodigoStatus { get; private set; }

    private RegistroAuditoria() { }

    public RegistroAuditoria(
        string tipoEvento,
        string descricao,
        Guid? usuarioId = null,
        string? papel = null,
        string? metodoHttp = null,
        string? caminhoRequisicao = null,
        int? codigoStatus = null)
    {
        if (string.IsNullOrWhiteSpace(tipoEvento))
            throw new ArgumentException("Tipo do evento é obrigatório.", nameof(tipoEvento));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição do evento é obrigatória.", nameof(descricao));

        Id = Guid.NewGuid();
        DataHoraUtc = DateTime.UtcNow;
        TipoEvento = tipoEvento;
        Descricao = descricao;
        UsuarioId = usuarioId;
        Papel = papel;
        MetodoHttp = metodoHttp;
        CaminhoRequisicao = caminhoRequisicao;
        CodigoStatus = codigoStatus;
    }
}
