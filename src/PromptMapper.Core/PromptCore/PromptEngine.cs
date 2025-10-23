using System.Collections.Immutable;
using PromptMapper.Abstractions.PromptCore;
using PromptMapper.Core.Common;

namespace PromptMapper.Core.PromptCore;

public class PromptEngine<TResponse> : IPromptEngine<TResponse> where TResponse : class
{
    private readonly MessageEntry[] _messages;

    public PromptEngine(MessageEntry[] messages)
    {
        _messages = messages;
    }

    public IPromptEngine<TResponse> FillMessage<TTemplate>(TTemplate templateInstance, string? key = null) where TTemplate : class
    {
        var template = _messages.FirstOrDefault(m => m.TemplateType == typeof(TTemplate));
        template?.Render(templateInstance);
        return this;
    }

    public bool IsFilled()
    {
        return _messages.All(m => m.IsRendered);
    }

    public IReadOnlyList<PromptMessage> GetPrompt()
    {
        return _messages.Select(m => new PromptMessage(m.Role, m.RenderedMessage)).ToImmutableList();
    }
}