using System.Collections.Generic;

namespace PromptMapper.Abstractions.Metadata
{
    public class ModelPropertyMetadata
    {
        public bool IsIgnored { get; set; }

        public string JsonName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Constraints { get; set; } = string.Empty;

        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}