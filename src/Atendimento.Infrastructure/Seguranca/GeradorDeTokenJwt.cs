using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Atendimento.Application.Autenticacao;
using Atendimento.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Atendimento.Infrastructure.Seguranca;

public class GeradorDeTokenJwt : IGeradorDeToken
{
    private readonly OpcoesJwt _opcoes;

    public GeradorDeTokenJwt(IOptions<OpcoesJwt> opcoes)
    {
        _opcoes = opcoes.Value;
    }

    public (string AccessToken, DateTime ExpiraEm) Gerar(Guid usuarioId, string email, string nome, string papel, Guid? chamadoId)
    {
        var expiraEm = DateTime.UtcNow.AddMinutes(_opcoes.ExpiracaoMinutos);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, nome),
            new(ClaimTypes.Role, papel)
        };

        if (chamadoId.HasValue)
            claims.Add(new Claim(ClaimsPersonalizadas.ChamadoId, chamadoId.Value.ToString()));

        var chaveAssinatura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opcoes.Chave));
        var credenciais = new SigningCredentials(chaveAssinatura, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opcoes.Emissor,
            audience: _opcoes.Audiencia,
            claims: claims,
            expires: expiraEm,
            signingCredentials: credenciais);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiraEm);
    }
}
