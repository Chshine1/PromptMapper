using PromptMapper.Abstractions.Interfaces.MessageTemplate;
using PromptMapper.Abstractions.Metadata;

namespace PromptMapper.Core.MessageTemplate;

public class MessageTemplateCompiler : IMessageTemplateCompiler
{
    private readonly IMetadataExtractor _metadataExtractor;
    
    public MessageTemplateCompiler(IMetadataExtractor metadataExtractor)
    {
        _metadataExtractor = metadataExtractor;
    }
    
    public IMessageTemplate<TTemplate> Compile<TTemplate>() where TTemplate : class
    {
        var metadata = _metadataExtractor.GetMetadata(typeof(TTemplate));
        if (!metadata.IsMessageTemplate || string.IsNullOrWhiteSpace(metadata.Template)) throw new InvalidOperationException($"Template '{typeof(TTemplate).Name}' is not a request template");
        return new MessageTemplate<TTemplate>(metadata.Template);
    }
}