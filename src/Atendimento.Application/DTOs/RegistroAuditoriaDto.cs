namespace Atendimento.Application.DTOs;

public record RegistroAuditoriaDto(
    Guid Id,
    DateTime DataHoraUtc,
    string TipoEvento,
    string Descricao,
    Guid? UsuarioId,
    string? Papel,
    string? MetodoHttp,
    string? CaminhoRequisicao,
    int? CodigoStatus);
