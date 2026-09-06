using Atendimento.Domain.Enums;

namespace Atendimento.Domain.Entities;

public class Chamado
{
    private readonly List<MensagemChamado> _mensagens = new();

    public Guid Id { get; private set; }
    public int Numero { get; private set; }
    public string Titulo { get; private set; } = null!;
    public string Descricao { get; private set; } = null!;
    public CategoriaChamado Categoria { get; private set; }
    public StatusChamado Status { get; private set; }
    public Guid AlunoId { get; private set; }
    public Guid? AtendenteId { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? ConcluidoEm { get; private set; }
    public IReadOnlyCollection<MensagemChamado> Mensagens => _mensagens.AsReadOnly();

    private Chamado() { }

    public Chamado(string titulo, string descricao, CategoriaChamado categoria, Guid alunoId)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título do chamado é obrigatório.", nameof(titulo));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição do chamado é obrigatória.", nameof(descricao));

        if (alunoId == Guid.Empty)
            throw new ArgumentException("Chamado deve estar vinculado a um aluno.", nameof(alunoId));

        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        Categoria = categoria;
        AlunoId = alunoId;
        Status = StatusChamado.Aberto;
        CriadoEm = DateTime.UtcNow;
    }

    public void IniciarAtendimento(Guid atendenteId)
    {
        if (Status != StatusChamado.Aberto)
            throw new InvalidOperationException("Somente chamados abertos podem entrar em atendimento.");

        if (atendenteId == Guid.Empty)
            throw new ArgumentException("Atendente inválido.", nameof(atendenteId));

        AtendenteId = atendenteId;
        Status = StatusChamado.EmAndamento;
    }

    public void AdicionarMensagem(string autor, string texto)
    {
        if (Status == StatusChamado.Concluido || Status == StatusChamado.Cancelado)
            throw new InvalidOperationException("Não é possível adicionar mensagens a um chamado finalizado.");

        _mensagens.Add(new MensagemChamado(autor, texto));
    }

    public void Concluir()
    {
        if (Status != StatusChamado.EmAndamento)
            throw new InvalidOperationException("Somente chamados em andamento podem ser concluídos.");

        Status = StatusChamado.Concluido;
        ConcluidoEm = DateTime.UtcNow;
    }

    public void Cancelar()
    {
        if (Status == StatusChamado.Concluido || Status == StatusChamado.Cancelado)
            throw new InvalidOperationException("Chamado já finalizado não pode ser cancelado.");

        Status = StatusChamado.Cancelado;
        ConcluidoEm = DateTime.UtcNow;
    }
}
