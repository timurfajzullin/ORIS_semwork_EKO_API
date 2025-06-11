using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Eko.Common.Ai;

public class AiRequest
{
    public readonly HttpClient _httpClient;
    public readonly ILogger<AiRequest> _logger;

    public AiRequest(IHttpClientFactory httpClientFactory, ILogger<AiRequest> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        
        _httpClient.BaseAddress = new Uri(AiOptions.BaseUrl);
    }

    public async Task<string> GetResponse(string prompt)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/generate");
        var requestBody = new
        {
            model = "deepseek-r1:7b",
            prompt = prompt,
        };

        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");
        
        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        // Обработка потокового ответа
        var responseBuilder = new StringBuilder();
        using (var reader = new StringReader(responseContent))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(line);
                        if (jsonDoc.RootElement.TryGetProperty("response", out var responseElement))
                        {
                            responseBuilder.Append(responseElement.GetString());
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "Error parsing JSON line");
                    }
                }
            }
        }

        return responseBuilder.ToString().Split("</think>")[1];
    }
}