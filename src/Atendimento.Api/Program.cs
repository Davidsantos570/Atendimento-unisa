using System.Text;
using Atendimento.Api.Middlewares;
using Atendimento.Api.Swagger;
using Atendimento.Application.Interfaces;
using Atendimento.Application.Services;
using Atendimento.Domain.Repositories;
using Atendimento.Infrastructure.IA;
using Atendimento.Infrastructure.Persistence;
using Atendimento.Infrastructure.Repositories;
using Atendimento.Infrastructure.Seguranca;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opcoes =>
{
    opcoes.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT recebido no login (sem o prefixo 'Bearer ')."
    });

    opcoes.OperationFilter<RequisitoDeAutorizacaoOperationFilter>();
});
builder.Services.AddExceptionHandler<TratamentoDeExcecoesGlobal>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<AtendimentoDbContext>(opcoes =>
    opcoes.UseSqlServer(builder.Configuration.GetConnectionString("AtendimentoDb")));

builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IAtendenteRepository, AtendenteRepository>();
builder.Services.AddScoped<IChamadoService, ChamadoService>();
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IAtendenteService, AtendenteService>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddSingleton<IHashDeSenha, HashDeSenhaBCrypt>();
builder.Services.AddSingleton<IGeradorDeToken, GeradorDeTokenJwt>();

builder.Services.Configure<OpcoesGemini>(builder.Configuration.GetSection(OpcoesGemini.Secao));
builder.Services.AddHttpClient<IAssistenteRespostaService, AssistenteRespostaGemini>(cliente =>
    cliente.BaseAddress = new Uri("https://generativelanguage.googleapis.com/"));

var opcoesJwt = builder.Configuration.GetSection(OpcoesJwt.Secao).Get<OpcoesJwt>() ?? new OpcoesJwt();
builder.Services.Configure<OpcoesJwt>(builder.Configuration.GetSection(OpcoesJwt.Secao));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = opcoesJwt.Emissor,
            ValidAudience = opcoesJwt.Audiencia,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opcoesJwt.Chave))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
