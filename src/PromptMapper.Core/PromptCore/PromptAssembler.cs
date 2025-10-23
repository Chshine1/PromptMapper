using PromptMapper.Abstractions.MessageTemplates;
using PromptMapper.Abstractions.PromptCore;
using PromptMapper.Abstractions.ResponseFormats;
using PromptMapper.Core.Common;

namespace PromptMapper.Core.PromptCore;

public class PromptAssembler : IPromptAssembler
{
    private readonly IMessageTemplateCompiler _messageTemplateCompiler;
    private readonly IResponseMessageCompiler _responseMessageCompiler;
    
    private readonly List<MessageEntry> _messages = new();
    
    public PromptAssembler(IMessageTemplateCompiler messageTemplateCompiler, IResponseMessageCompiler responseMessageCompiler)
    {
        _messageTemplateCompiler = messageTemplateCompiler;
        _responseMessageCompiler = responseMessageCompiler;
    }
    
    public IPromptAssembler WithMessageTemplate<TTemplate>(string role, string? key = null) where TTemplate : class
    {
        var template = _messageTemplateCompiler.Compile<TTemplate>();
        _messages.Add(new DynamicMessageEntry<TTemplate>(role, template));
        return this;
    }

    public IPromptAssembler WithStaticMessage(string role, string message)
    {
        _messages.Add(new StaticMessageEntry(role, message));
        return this;
    }

    public IPromptEngine<TResponse> WithResponseFormat<TResponse>() where TResponse : class
    {
        _messages.Add(new StaticMessageEntry(PromptMessage.Roles.System, _responseMessageCompiler.Compile(typeof(TResponse))));
        return new PromptEngine<TResponse>(_messages.ToArray());
    }
}