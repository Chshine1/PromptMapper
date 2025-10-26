using System.Reflection;
using System.Text.Json.Serialization;
using PromptMapper.Abstractions.MessageTemplates.Attributes;
using PromptMapper.Abstractions.Metadata;
using PromptMapper.Abstractions.ResponseFormats.Attributes;

namespace PromptMapper.Core.Metadata;

public class MetadataExtractor : IMetadataExtractor
{
    private readonly Dictionary<Type, ModelMetadata> _modelMetadata = new();
    
    private readonly IEnumerable<IMetadataExtender> _extenders;
    
    public MetadataExtractor(IEnumerable<IMetadataExtender>? extenders = null)
    {
        _extenders = extenders?.OrderBy(e => e.Order) ?? Enumerable.Empty<IMetadataExtender>();
    }
    
    public ModelMetadata GetMetadata(Type type)
    {
        if (_modelMetadata.TryGetValue(type, out var modelMetadata)) return modelMetadata;
        
        var metadata = new ModelMetadata();
        
        var responseAttribute = type.GetCustomAttribute<ResponseFormatAttribute>();
        if (responseAttribute != null)
        {
            metadata.IsResponseFormat = true;
            
            metadata.Description = responseAttribute.Description;

            metadata.Properties = type.GetProperties()
                .Select(GetModelMetadata)
                .ToList();
        
            return metadata;
        }

        var requestAttributes = type.GetCustomAttributes<MessageTemplateAttribute>();
        foreach (var requestAttribute in requestAttributes)
        {
            metadata.IsMessageTemplate = true;
            metadata.TemplateName = string.IsNullOrWhiteSpace(requestAttribute.TemplateName) ? type.Name : requestAttribute.TemplateName;
            if (!string.IsNullOrWhiteSpace(requestAttribute.Key)) metadata.Keys.Add(requestAttribute.Key);
        }
        
        _modelMetadata.Add(type, metadata);

        return metadata;
    }

    private ModelPropertyMetadata GetModelMetadata(PropertyInfo property)
    {
        var metadata = new ModelPropertyMetadata
        {
            IsIgnored = false
        };
        
        var jsonIgnoreAttribute = property.GetCustomAttribute<JsonIgnoreAttribute>();
        if (jsonIgnoreAttribute != null)
        {
            metadata.IsIgnored = true;
        }
        
        var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
        metadata.JsonName = jsonPropertyNameAttribute?.Name ?? property.Name;

        var propertyAttribute = property.GetCustomAttribute<ResponsePropertyAttribute>();
        if (propertyAttribute == null)
        {
            return metadata;
        }
        metadata.Description = propertyAttribute.Description;
        metadata.Constraints = propertyAttribute.Constraints;

        foreach (var extender in _extenders)
        {
            extender.ExtendPropertyMetadata(property, metadata);
        }
        return metadata;
    }
}