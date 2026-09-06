using System.Security.Claims;
using Atendimento.Application.Auditoria;
using Atendimento.Application.Interfaces;

namespace Atendimento.Api.Middlewares;

public class RegistroDeRequisicaoMiddleware
{
    private readonly RequestDelegate _proximo;

    public RegistroDeRequisicaoMiddleware(RequestDelegate proximo)
    {
        _proximo = proximo;
    }

    public async Task InvokeAsync(HttpContext context, IAuditoriaService auditoriaService)
    {
        await _proximo(context);

        if (!context.Request.Path.StartsWithSegments("/api"))
            return;

        Guid? usuarioId = Guid.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
        var papel = context.User.FindFirstValue(ClaimTypes.Role);

        await auditoriaService.RegistrarAsync(
            TiposDeEventoAuditoria.Requisicao,
            $"{context.Request.Method} {context.Request.Path} -> {context.Response.StatusCode}",
            usuarioId,
            papel,
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode);
    }
}
