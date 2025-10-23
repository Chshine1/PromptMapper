namespace PromptMapper.Abstractions.MessageTemplates
{
    public interface IMessageTemplate<in TTemplate> where TTemplate : class
    {
        string Render(TTemplate templateInstance);
    }
}