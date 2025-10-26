namespace PromptMapper.Abstractions.MessageTemplates
{
    public interface IMessageTemplateCompiler
    {
        IMessageTemplate<TTemplate> Compile<TTemplate>(string? key = null) where TTemplate : class;
    }
}