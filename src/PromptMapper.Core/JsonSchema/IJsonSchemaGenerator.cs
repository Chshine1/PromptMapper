namespace PromptMapper.Core.JsonSchema;

public interface IJsonSchemaGenerator
{
    string GenerateJsonSchema(Type type);
}