using System;

namespace PromptMapper.Abstractions.Interfaces
{
    public interface IJsonSchemaGenerator
    {
        string GenerateJsonSchema(Type type);
    }
}