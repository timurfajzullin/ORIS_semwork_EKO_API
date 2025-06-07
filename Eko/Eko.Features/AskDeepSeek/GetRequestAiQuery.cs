using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Eko.Common.Cqrs;
using Microsoft.Extensions.Configuration;

namespace Eko.Features.AskDeepSeek;

public class GetRequestAiQuery : IQuery<string>
{
    public string Query { get; set; }
}

public class GetRequestAiQueryHandler : IQueryHandler<GetRequestAiQuery, string>
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;

    public GetRequestAiQueryHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiKey = "sk-143d5836a1a34f1ab98914b8e2f6f9c7";
            
        if (string.IsNullOrEmpty(_apiKey))
            throw new InvalidOperationException("DeepSeek API key is not configured");

        // Эти настройки можно перенести в AddHttpClient если используете именованный клиент
        _httpClient.BaseAddress = new Uri("https://api.deepseek.com/v1/");
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _apiKey);
    }
    
    public async Task<string> Execute(GetRequestAiQuery command)
    {
        if (string.IsNullOrEmpty(_apiKey))
            throw new InvalidOperationException("DeepSeek API key is not configured");

        var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var requestBody = new
        {
            model = "deepseek-chat",
            messages = new[] { new { role = "user", content = command.Query } },
            temperature = 0.7
        };

        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        try
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseContent);
                
            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"API request failed: {ex.Message}", ex);
        }
    }
}