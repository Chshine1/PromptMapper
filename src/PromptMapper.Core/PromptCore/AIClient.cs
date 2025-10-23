using System.Net.Http.Json;
using PromptMapper.Abstractions.PromptCore;

namespace PromptMapper.Core.PromptCore;

public class AIClient : IAIClient
{
    private readonly HttpClient _httpClient = new();
    
    public async Task<string> SendPromptAsync(IReadOnlyList<PromptMessage> messages)
    {
        var request = new
        {
            model = Environment.GetEnvironmentVariable("AI_MODEL"), messages
        };
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("AI_API_KEY")}");
        
        return await (await _httpClient.PostAsJsonAsync(Environment.GetEnvironmentVariable("AI_API_ENDPOINT"), request)).Content.ReadAsStringAsync();
    }
}