namespace PromptMapper.Abstractions.Interfaces.MessageTemplate
{
    public interface IMessageTemplateCompiler
    {
        IMessageTemplate<TTemplate> Compile<TTemplate>() where TTemplate : class;
    }
}