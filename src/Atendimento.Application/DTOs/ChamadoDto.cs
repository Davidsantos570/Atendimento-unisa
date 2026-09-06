using Atendimento.Domain.Enums;

namespace Atendimento.Application.DTOs;

public record ChamadoDto(
    Guid Id,
    int Numero,
    string Titulo,
    string Descricao,
    CategoriaChamado Categoria,
    StatusChamado Status,
    Guid AlunoId,
    Guid? AtendenteId,
    DateTime CriadoEm,
    DateTime? ConcluidoEm,
    IReadOnlyCollection<MensagemDto> Mensagens);

public record MensagemDto(string Autor, string Texto, DateTime EnviadaEm);

public record AbrirChamadoDto(string Titulo, string Descricao, CategoriaChamado Categoria);

public record AdicionarMensagemDto(string Texto);

public record SugestaoIaDto(string Sugestao);
