using System.Reflection;
using PromptMapper.Abstractions.Metadata;

namespace PromptMapper.Core.Implementations;

public class JsonMetadataExtender : IMetadataExtender
{
    public int Order => 100;
    
    public void ExtendPropertyMetadata(PropertyInfo property, ModelPropertyMetadata metadata)
    {
        var jsonIgnoreAttribute = property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>();
        if (jsonIgnoreAttribute != null)
        {
            metadata.IsIgnored = true;
        }
        
        var jsonPropertyNameAttribute = property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>();
        metadata.JsonName = jsonPropertyNameAttribute?.Name ?? property.Name;
    }
}