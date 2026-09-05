using Atendimento.Domain.Enums;

namespace Atendimento.Application.DTOs;

public record ChamadoDto(
    Guid Id,
    string Titulo,
    string Descricao,
    StatusChamado Status,
    Guid AlunoId,
    Guid? AtendenteId,
    DateTime CriadoEm,
    DateTime? ConcluidoEm,
    IReadOnlyCollection<MensagemDto> Mensagens);

public record MensagemDto(string Autor, string Texto, DateTime EnviadaEm);

public record AbrirChamadoDto(string Titulo, string Descricao, Guid AlunoId);
