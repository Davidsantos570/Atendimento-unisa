using Atendimento.Application.Interfaces;
using Atendimento.Application.Services;
using Atendimento.Domain.Repositories;
using Atendimento.Infrastructure.IA;
using Atendimento.Infrastructure.Persistence;
using Atendimento.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AtendimentoDbContext>(opcoes =>
    opcoes.UseSqlServer(builder.Configuration.GetConnectionString("AtendimentoDb")));

builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IAtendenteRepository, AtendenteRepository>();
builder.Services.AddScoped<IChamadoService, ChamadoService>();

builder.Services.Configure<OpcoesGemini>(builder.Configuration.GetSection(OpcoesGemini.Secao));
builder.Services.AddHttpClient<IAssistenteRespostaService, AssistenteRespostaGemini>(cliente =>
    cliente.BaseAddress = new Uri("https://generativelanguage.googleapis.com/"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
