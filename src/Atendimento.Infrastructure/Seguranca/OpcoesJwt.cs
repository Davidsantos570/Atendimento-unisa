namespace Atendimento.Infrastructure.Seguranca;

public class OpcoesJwt
{
    public const string Secao = "Jwt";

    public string Chave { get; set; } = string.Empty;
    public string Emissor { get; set; } = string.Empty;
    public string Audiencia { get; set; } = string.Empty;
    public int ExpiracaoMinutos { get; set; } = 60;
}
