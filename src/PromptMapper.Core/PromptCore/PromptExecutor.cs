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
        return JsonSerializer.Deserialize<TResponse>(await _client.SendPromptAsync(messages))!;
    }
}