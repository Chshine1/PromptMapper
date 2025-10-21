using System;

namespace PromptMapper.Abstractions.Interfaces.MessageTemplate
{
    public interface IMessageTemplate
    {
        string Render(object instance);
        Type TemplateType { get; }
    }
    
    public interface IMessageTemplate<in TTemplate> : IMessageTemplate where TTemplate : class
    {
        string Render(TTemplate templateInstance);
    }
}