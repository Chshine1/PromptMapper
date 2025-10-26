using System;

namespace PromptMapper.Abstractions.MessageTemplates.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageTemplateAttribute : Attribute
    {
        public string TemplateName { get; set; } = string.Empty;
        
        public string Key { get; set; } = string.Empty;
    }
}