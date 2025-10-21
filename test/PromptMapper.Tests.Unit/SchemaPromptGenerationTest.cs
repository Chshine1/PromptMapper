using System.Text.Json.Serialization;
using OpenAI.Chat;
using PromptMapper.Abstractions.Metadata.Attributes;
using PromptMapper.Core.Implementations;
using PromptMapper.Core.MessageTemplate;
using PromptMapper.Core.Metadata;
using PromptMapper.SystemTextJson;
using Xunit.Abstractions;

namespace PromptMapper.Tests.Unit;

public class SchemaPromptGenerationTest(ITestOutputHelper testOutputHelper)
{
    [MessageTemplate(Template = "You are a helpful assistant, take the number {TestNumber}, and return me with it added by 1.")]
    private class TestPrompt
    {
        public int TestNumber { get; set; }
    }

    [ResponseFormat]
    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestModel
    {
        [JsonPropertyName("test_property")]
        [ResponseProperty(Description = "an integer where you put your result")]
        public int TestProperty { get; set; }

        [JsonIgnore] public string FuckYou { get; set; } = string.Empty;
    }

    [Fact]
    public void GenerateJsonSchema_Success_Test()
    {
        var generator = new JsonSchemaGenerator();
        var result = generator.GenerateJsonSchema(typeof(TestModel));
        testOutputHelper.WriteLine(result);
    }

    [Fact]
    public void GenerateSchemaPrompt_Success_Test()
    {
        var metadataProvider = new MetadataExtractor([new JsonMetadataExtender()]);
        var jsonGenerator = new JsonSchemaGenerator();
        var schemaGenerator = new ResponseMessageCompiler(jsonGenerator, metadataProvider);
        testOutputHelper.WriteLine(schemaGenerator.Compile(typeof(TestModel)));
    }

    [Fact]
    public void GenerateFullPrompt_Success_Test()
    {
        var metadataProvider = new MetadataExtractor([new JsonMetadataExtender()]);
        var jsonGenerator = new JsonSchemaGenerator();
        var schemaGenerator = new ResponseMessageCompiler(jsonGenerator, metadataProvider);
        var templateGenerator = new MessageTemplateCompiler(metadataProvider);
        
        var template = new PromptAssembler(templateGenerator, schemaGenerator)
            .WithMessageTemplate<TestPrompt>(ChatMessageRole.System)
            .WithResponseFormat<TestModel>();
        var prompt = template
            .FillMessage(new TestPrompt { TestNumber = 1 })
            .GetPrompt();
        testOutputHelper.WriteLine(prompt);
    }
}