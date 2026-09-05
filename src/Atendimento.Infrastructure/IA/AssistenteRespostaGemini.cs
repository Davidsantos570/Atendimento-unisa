using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Atendimento.Application.Interfaces;
using Atendimento.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Atendimento.Infrastructure.IA;

public class AssistenteRespostaGemini : IAssistenteRespostaService
{
    private static readonly JsonSerializerOptions OpcoesJson = new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient _httpClient;
    private readonly OpcoesGemini _opcoes;

    public AssistenteRespostaGemini(HttpClient httpClient, IOptions<OpcoesGemini> opcoes)
    {
        _httpClient = httpClient;
        _opcoes = opcoes.Value;
    }

    public async Task<string> GerarRespostaAsync(Chamado chamado, string mensagemRecebida)
    {
        var requisicao = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[] { new { text = MontarPrompt(chamado, mensagemRecebida) } }
                }
            }
        };

        var url = $"v1beta/models/{_opcoes.Modelo}:generateContent?key={_opcoes.ChaveApi}";
        var resposta = await _httpClient.PostAsJsonAsync(url, requisicao);
        resposta.EnsureSuccessStatusCode();

        var corpo = await resposta.Content.ReadFromJsonAsync<RespostaGemini>(OpcoesJson);

        return corpo?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text
               ?? "Não foi possível gerar uma resposta automática no momento.";
    }

    private static string MontarPrompt(Chamado chamado, string mensagemRecebida)
    {
        var historico = string.Join(
            "\n",
            chamado.Mensagens.Select(mensagem => $"{mensagem.Autor}: {mensagem.Texto}"));

        return $"""
            Você é o assistente virtual de atendimento ao aluno da Unisa.
            Chamado: {chamado.Titulo}
            Descrição: {chamado.Descricao}
            Histórico da conversa:
            {historico}
            Nova mensagem do aluno: {mensagemRecebida}

            Responda de forma clara, cordial e objetiva, em português.
            """;
    }

    private record RespostaGemini([property: JsonPropertyName("candidates")] List<CandidatoGemini>? Candidates);

    private record CandidatoGemini([property: JsonPropertyName("content")] ConteudoGemini? Content);

    private record ConteudoGemini([property: JsonPropertyName("parts")] List<ParteGemini>? Parts);

    private record ParteGemini([property: JsonPropertyName("text")] string? Text);
}
