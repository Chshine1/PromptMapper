using System.Collections.Generic;

namespace PromptMapper.Abstractions.Metadata
{
    public class ModelMetadata
    {
        public bool IsResponseFormat { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<ModelPropertyMetadata> Properties { get; set; } = new List<ModelPropertyMetadata>();
        
        public bool IsMessageTemplate { get; set; }
        public string Template { get; set; } = string.Empty;
    }
}