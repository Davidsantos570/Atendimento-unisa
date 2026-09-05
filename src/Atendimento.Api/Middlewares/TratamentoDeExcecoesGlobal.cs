using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Atendimento.Api.Middlewares;

public class TratamentoDeExcecoesGlobal : IExceptionHandler
{
    private readonly ILogger<TratamentoDeExcecoesGlobal> _logger;

    public TratamentoDeExcecoesGlobal(ILogger<TratamentoDeExcecoesGlobal> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception excecao, CancellationToken cancellationToken)
    {
        var (statusCode, titulo) = excecao switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, excecao.Message),
            InvalidOperationException => (StatusCodes.Status409Conflict, excecao.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, excecao.Message),
            _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado. Tente novamente mais tarde.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(excecao, "Erro não tratado ao processar a requisição.");

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = titulo
        }, cancellationToken);

        return true;
    }
}
