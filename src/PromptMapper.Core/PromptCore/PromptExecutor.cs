using System.Text.Json;
using PromptMapper.Abstractions.PromptCore;

namespace PromptMapper.Core.PromptCore;

public class PromptExecutor<TResponse> : IPromptExecutor<TResponse> where TResponse : class
{
    private readonly IPromptEngine<TResponse> _engine;
    private readonly IAIClient _client;

    public PromptExecutor(IPromptEngine<TResponse> engine, IAIClient client)
    {
        _engine = engine;
        _client = client;
    }

    public async Task<TResponse> ExecuteAsync()
    {
        var messages = _engine.GetPrompt();
        var result = await _client.SendPromptAsync(messages);
        
        using var document = JsonDocument.Parse(result);
        var root = document.RootElement;

        var choices = root.GetProperty("choices");
        var firstChoice = choices[0];
        var messageElement = firstChoice.GetProperty("message").GetProperty("content");
        return JsonSerializer.Deserialize<TResponse>(messageElement.GetString()!)!;
    }
}