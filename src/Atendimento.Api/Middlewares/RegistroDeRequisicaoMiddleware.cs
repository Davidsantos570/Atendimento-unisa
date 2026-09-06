using System.Security.Claims;
using Atendimento.Application.Auditoria;
using Atendimento.Application.Interfaces;

namespace Atendimento.Api.Middlewares;

public class RegistroDeRequisicaoMiddleware
{
    private readonly RequestDelegate _proximo;
    private readonly IServiceScopeFactory _scopeFactory;

    public RegistroDeRequisicaoMiddleware(RequestDelegate proximo, IServiceScopeFactory scopeFactory)
    {
        _proximo = proximo;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _proximo(context);

        if (!context.Request.Path.StartsWithSegments("/api"))
            return;

        Guid? usuarioId = Guid.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
        var papel = context.User.FindFirstValue(ClaimTypes.Role);

        using var escopo = _scopeFactory.CreateScope();
        var auditoriaService = escopo.ServiceProvider.GetRequiredService<IAuditoriaService>();

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
