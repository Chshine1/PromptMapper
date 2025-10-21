using OpenAI.Chat;
using PromptMapper.Abstractions.Interfaces;
using PromptMapper.Abstractions.Interfaces.MessageTemplate;
using PromptMapper.Abstractions.Interfaces.ResponseFormat;

namespace PromptMapper.Core.Implementations;

public class PromptAssembler : IPromptAssembler
{
    private readonly IMessageTemplateCompiler _messageTemplateCompiler;
    private readonly IResponseMessageCompiler _responseMessageCompiler;
    
    private readonly Dictionary<Type, IMessageTemplate> _messages = new();
    
    public PromptAssembler(IMessageTemplateCompiler messageTemplateCompiler, IResponseMessageCompiler responseMessageCompiler)
    {
        _messageTemplateCompiler = messageTemplateCompiler;
        _responseMessageCompiler = responseMessageCompiler;
    }
    
    public IPromptAssembler WithMessageTemplate<TTemplate>(ChatMessageRole role, string? key = null) where TTemplate : class
    {
        _messages.Add(typeof(TTemplate), _messageTemplateCompiler.Compile<TTemplate>());
        return this;
    }

    public IPromptAssembler WithStaticMessage(ChatMessageRole role, string message)
    {
        throw new NotImplementedException();
    }

    public IPromptEngine<TResponse> WithResponseFormat<TResponse>() where TResponse : class
    {
        return new PromptEngine<TResponse>(_messages, _responseMessageCompiler.Compile(typeof(TResponse)));
    }
}