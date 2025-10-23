using System;

namespace PromptMapper.Abstractions.ResponseFormats.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ResponseFormatAttribute : Attribute
    {
        public string Description { get; set; } = string.Empty;
    }
}