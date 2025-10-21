using PromptMapper.Abstractions.Interfaces;
using PromptMapper.Abstractions.Interfaces.ResponseFormat;
using PromptMapper.Abstractions.Metadata;

namespace PromptMapper.Core.Implementations;

public class ResponseMessageCompiler : IResponseMessageCompiler
{
    private readonly IJsonSchemaGenerator _jsonSchemaGenerator;
    private readonly IMetadataExtractor _metadataExtractor;
    
    public ResponseMessageCompiler(IJsonSchemaGenerator generator, IMetadataExtractor metadataExtractor)
    {
        _jsonSchemaGenerator = generator;
        _metadataExtractor = metadataExtractor;
    }
    
    public string Compile(Type type)
    {
        var schema = _jsonSchemaGenerator.GenerateJsonSchema(type);
        var metadata = _metadataExtractor.GetMetadata(type);
        return $"Respond strictly with JSON matching this schema:\n{schema}\n{GenerateDescription(metadata)}";
    }

    public IResponseMessage CompileA(Type type)
    {
        throw new NotImplementedException();
    }

    private static string GenerateDescription(ModelMetadata metadata)
    {
        var description = "";
        if (!string.IsNullOrWhiteSpace(metadata.Description))
        {
            description = $"The response represents:\n- {metadata.Description}\n";
        }

        if (metadata.Properties.Count == 0)
        {
            description += "Your response must be a strict JSON matching this schema.";
            return description;
        }

        description += "The properties' descriptions and constraints are as follows:\n";
        foreach (var property in metadata.Properties)
        {
            if (property.IsIgnored) continue;
            
            var hasConstraints = !string.IsNullOrWhiteSpace(property.Constraints);
            if (!string.IsNullOrWhiteSpace(property.Description))
            {
                description += $"- {property.JsonName}: {property.Description}";
                if (hasConstraints)
                {
                    description += $", with constraints {property.Constraints}.\n";
                }
                else
                {
                    description += ".\n";
                }
            }
            else if (hasConstraints)
            {
                description += $"- {property.JsonName}: has constraints {property.Constraints}.\n";
            }
        }

        description += "Your response must be a strict JSON matching this schema.";
        return description;
    }
}