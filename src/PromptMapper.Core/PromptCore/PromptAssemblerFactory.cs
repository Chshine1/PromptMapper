using PromptMapper.Abstractions.MessageTemplates;
using PromptMapper.Abstractions.PromptCore;
using PromptMapper.Abstractions.ResponseFormats;

namespace PromptMapper.Core.PromptCore;

public class PromptAssemblerFactory
{
    private readonly IMessageTemplateCompiler _templateCompiler;
    private readonly IResponseMessageCompiler _responseCompiler;
    
    public PromptAssemblerFactory(IMessageTemplateCompiler templateCompiler, IResponseMessageCompiler responseCompiler)
    {
        _templateCompiler = templateCompiler;
        _responseCompiler = responseCompiler;
    }

    public IPromptAssembler Create()
    {
        return new PromptAssembler(_templateCompiler, _responseCompiler);
    }
}