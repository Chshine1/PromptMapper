namespace PromptMapper.Abstractions.MessageTemplates
{
    public interface IMessageTemplateCompiler
    {
        IMessageTemplate<TTemplate> Compile<TTemplate>(string? template = null) where TTemplate : class;
    }
}