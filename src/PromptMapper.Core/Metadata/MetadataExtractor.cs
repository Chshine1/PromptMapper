using System.Reflection;
using PromptMapper.Abstractions.Metadata;
using PromptMapper.Abstractions.Metadata.Attributes;

namespace PromptMapper.Core.Metadata;

public class MetadataExtractor : IMetadataExtractor
{
    private readonly Dictionary<Type, ModelMetadata> _modelMetadata = new();
    
    private readonly IEnumerable<IMetadataExtender> _extenders;
    
    public MetadataExtractor(IEnumerable<IMetadataExtender>? extenders)
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

        var requestAttribute = type.GetCustomAttribute<MessageTemplateAttribute>();
        if (requestAttribute != null)
        {
            metadata.IsMessageTemplate = true;
            metadata.Template = requestAttribute.Template;
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