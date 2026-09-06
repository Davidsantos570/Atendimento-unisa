using System.Text;
using System.Text.Json.Serialization;
using Atendimento.Api.Middlewares;
using Atendimento.Api.Swagger;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Entities;
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

builder.Services.AddControllers()
    .AddJsonOptions(opcoes => opcoes.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
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
builder.Services.AddScoped<IConvidadoRepository, ConvidadoRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IChamadoService, ChamadoService>();
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IAtendenteService, AtendenteService>();
builder.Services.AddScoped<IConvidadoService, ConvidadoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IRegistroAuditoriaRepository, RegistroAuditoriaRepository>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
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

const string PoliticaCorsFrontend = "FrontendDev";
builder.Services.AddCors(opcoes =>
{
    opcoes.AddPolicy(PoliticaCorsFrontend, politica => politica
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<RegistroDeRequisicaoMiddleware>();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(PoliticaCorsFrontend);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var escopoSeed = app.Services.CreateScope())
{
    var adminRepository = escopoSeed.ServiceProvider.GetRequiredService<IAdminRepository>();

    if (!await adminRepository.ExisteAlgumAsync())
    {
        var emailAdmin = app.Configuration["AdminSeed:Email"];
        var senhaAdmin = app.Configuration["AdminSeed:Senha"];

        if (!string.IsNullOrWhiteSpace(emailAdmin) && !string.IsNullOrWhiteSpace(senhaAdmin))
        {
            var nomeAdmin = app.Configuration["AdminSeed:Nome"] ?? "Administrador";
            var hashDeSenha = escopoSeed.ServiceProvider.GetRequiredService<IHashDeSenha>();
            var admin = new Admin(nomeAdmin, emailAdmin, hashDeSenha.GerarHash(senhaAdmin));
            await adminRepository.AdicionarAsync(admin);
        }
    }
}

app.Run();
