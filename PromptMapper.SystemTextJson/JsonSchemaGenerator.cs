#if NET9_0
using System.Text.Json;
using System.Text.Json.Schema;
#endif
using PromptMapper.Abstractions.Interfaces;

namespace PromptMapper.SystemTextJson;

public class JsonSchemaGenerator : IJsonSchemaGenerator
{
    public string GenerateJsonSchema(Type type)
    {
#if NET9_0
        var options = JsonSerializerOptions.Default;
        var exporterOptions = new JsonSchemaExporterOptions
        {
            TreatNullObliviousAsNonNullable = true,
        };
        var schema = options.GetJsonSchemaAsNode(type, exporterOptions);
        return schema.ToString();
#elif NET6_0
        throw new Exception("Test");
#endif
    }
}