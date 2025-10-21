using PromptMapper.Abstractions.Interfaces;
using PromptMapper.Abstractions.Interfaces.MessageTemplate;

namespace PromptMapper.Core.Implementations;

public class PromptEngine<TResponse> : IPromptEngine<TResponse> where TResponse : class
{
    private string _prompt = string.Empty;
    
    private readonly string _responseSchema;
    
    private readonly Dictionary<Type, IMessageTemplate> _messages;

    public PromptEngine(Dictionary<Type, IMessageTemplate> messages, string responseSchema)
    {
        _responseSchema = responseSchema;
        _messages = messages;
    }


    public IPromptEngine<TResponse> FillMessage<TTemplate>(TTemplate templateInstance, string? key = null) where TTemplate : class
    {
        if (_messages.TryGetValue(typeof(TTemplate), out var messageTemplate))
        {
            _prompt += messageTemplate.Render(templateInstance) + "\n";
        }
        return this;
    }

    public bool IsFilled()
    {
        throw new NotImplementedException();
    }

    public Task<TResponse> ExecuteAsync()
    {
        throw new NotImplementedException();
    }

    public string GetPrompt()
    {
        _prompt += _responseSchema;
        return _prompt;
    }
}