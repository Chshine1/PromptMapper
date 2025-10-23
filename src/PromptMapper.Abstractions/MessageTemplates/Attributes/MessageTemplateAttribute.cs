using System;

namespace PromptMapper.Abstractions.MessageTemplates.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MessageTemplateAttribute : Attribute
    {
        public string Template { get; set; } = string.Empty;
    }
}