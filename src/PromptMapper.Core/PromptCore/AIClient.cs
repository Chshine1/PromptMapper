using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PromptMapper.Abstractions.PromptCore;
using PromptMapper.Core.PromptCore.Configurations;

namespace PromptMapper.Core.PromptCore;

public class AIClient : IAIClient
{
    private readonly HttpClient _httpClient;
    private readonly AIClientOptions _options;
    
    public AIClient(IOptions<AIClientOptions> options, HttpClient? httpClient = null)
    {
        _options = options.Value;
        _httpClient = httpClient ?? new HttpClient();
    }
    
    public async Task<string> SendPromptAsync(IReadOnlyList<PromptMessage> messages)
    {
        var request = new
        {
            model = _options.Model, messages
        };
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_options.ApiKey}");
        
        var response = await (await _httpClient.PostAsJsonAsync(_options.BaseUrl, request)).Content.ReadAsStringAsync();
        
        using var document = JsonDocument.Parse(response);
        var root = document.RootElement;

        var choices = root.GetProperty("choices");
        var firstChoice = choices[0];
        var messageElement = firstChoice.GetProperty("message").GetProperty("content");

        return messageElement.GetString()!;
    }
}