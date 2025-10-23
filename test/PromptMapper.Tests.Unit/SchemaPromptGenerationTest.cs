using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using PromptMapper.Abstractions.MessageTemplates.Attributes;
using PromptMapper.Abstractions.PromptCore;
using PromptMapper.Abstractions.ResponseFormats.Attributes;
using PromptMapper.Core.PromptCore;
using PromptMapper.Extensions.DependencyInjection;
using Xunit.Abstractions;
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedMember.Local

namespace PromptMapper.Tests.Unit;

public class SchemaPromptGenerationTest(ITestOutputHelper testOutputHelper)
{
    [MessageTemplate(Template = "Take the number {TestNumber}, and return me with it added by 1.")]
    private class TestPrompt
    {
        public int TestNumber { get; set; }
    }

    [ResponseFormat]
    private class TestModel
    {
        [JsonPropertyName("test_property")]
        [ResponseProperty(Description = "an integer where you put your result")]
        public int TestProperty { get; set; }
        
        [JsonPropertyName("additional_info")]
        [ResponseProperty(Description = "anything you like", Constraints = "length should be smaller than 20")]
        public required string AdditionalInformation { get; set; }

        [JsonIgnore] public string FuckYou { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{{ TestProperty: {TestProperty}, AdditionalInformation: {AdditionalInformation} }}";
        }
    }
    
    [Fact]
    public async Task GenerateFullPrompt_Success_Test()
    {
        var serviceProvider = new ServiceCollection()
            .AddTemplateCore()
            .AddSingleton<IAIClient, AIClient>(_ => new AIClient())
            .BuildServiceProvider();
        
        var template = serviceProvider.GetRequiredService<IPromptAssembler>()
            .WithStaticMessage(PromptMessage.Roles.System, "You are a helpful assistant. Handle the user's task:")
            .WithMessageTemplate<TestPrompt>(PromptMessage.Roles.User)
            .WithResponseFormat<TestModel>();

        var e = template
            .FillMessage(new TestPrompt { TestNumber = 3 });
        var t = await new PromptExecutor<TestModel>(e, serviceProvider.GetRequiredService<IAIClient>()).ExecuteAsync();
        testOutputHelper.WriteLine(t.ToString());
    }
}