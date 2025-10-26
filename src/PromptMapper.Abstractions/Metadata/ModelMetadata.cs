using System.Collections.Generic;

namespace PromptMapper.Abstractions.Metadata
{
    public class ModelMetadata
    {
        public bool IsResponseFormat { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<ModelPropertyMetadata> Properties { get; set; } = new List<ModelPropertyMetadata>();
        
        public bool IsMessageTemplate { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public List<string> Keys { get; set; } = new List<string>();
    }
}