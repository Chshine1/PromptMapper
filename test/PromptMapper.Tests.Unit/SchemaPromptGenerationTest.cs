using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PromptMapper.Abstractions.MessageTemplates.Attributes;
using PromptMapper.Abstractions.PromptCore;
using PromptMapper.Abstractions.ResponseFormats.Attributes;
using PromptMapper.Core.PromptCore;
using PromptMapper.Core.PromptCore.Configurations;
using PromptMapper.Extensions.DependencyInjection;
using Xunit.Abstractions;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedMember.Local

namespace PromptMapper.Tests.Unit;

public class SchemaPromptGenerationTest(ITestOutputHelper testOutputHelper)
{
    private readonly ServiceProvider _serviceProvider = new ServiceCollection()
        .AddTemplateCore()
        .AddSingleton<IOptions<PromptOptions>>(new OptionsWrapper<PromptOptions>(new PromptOptions
        {
            TemplateDirectory = @"D:\RiderProjects\PromptMapper\test\PromptMapper.Tests.Unit"
        }))
        .BuildServiceProvider();

    [MessageTemplate(Key = "debug")]
    private class NarrationTemplate
    {
        public NarrationTemplate()
        {
            CurrentTurn = 0;
            MaxTurns = 10;
            NodeSummary = """
                          核心情节：颜舜华初入皇宫，作为新任伴读跟随引路嬷嬷前往凤仪宫。途中首次与姬瑶公主相遇，姬瑶热情主动地接纳她，展现出对陪伴的强烈渴望，并立即表达出希望与颜舜华建立深厚情谊的愿望。
                          关键对话：姬瑶认出颜舜华是父皇安排的新伴读，兴奋地称赞其容貌，并亲切要求称呼自己为“阿瑶”；她透露今日将去太学听晏丞相讲学，并分享自己在御花园的秘密据点——木槿花旁的秋千；最后真诚发问：“我们以后会是最好的朋友吗？就像亲姐妹那样？”
                          人物关系：姬瑶对颜舜华一见如故，表现出超乎主仆的亲近意愿，主动拉手、分享秘密，流露出依赖与信任；颜舜华从最初的拘谨逐渐被公主的真诚打动，态度由恭敬转为柔和，初步建立起情感连接。
                          情感基调：整体氛围明媚轻快，充满春日生机与童真温情。虽有深宫压抑的背景暗示，但姬瑶的活泼与颜舜华的温婉共同营造出温暖、希望的情感走向。
                          悬念设置：姬瑶对“最好朋友”的期待为两人未来的关系发展埋下伏笔；提及“说话有点凶”的晏丞相，暗示后续讲学场景可能存在冲突或考验；颜舜华能否真正融入宫廷生活、回应公主的情感依赖，成为潜在的成长线索。

                          以下是故事最后的三条对话

                          姬瑶：（兴致勃勃地）舜华姐姐你看！这花儿不就是你名字里的“舜华”？我最喜欢它了，开得又大又好看，还不娇气！（她凑近你，带着点小得意和分享秘密的语气）悄悄告诉你哦，这宫里我最喜欢的地方就是御花园西角那片小池塘边，那里有好多这种木槿花，还有我偷偷让人扎的秋千！改天我带你去看！
                          姬瑶：（她眨眨眼，满是期待）舜华姐姐，你说...我们以后会是最好的朋友吗？就像...就像亲姐妹那样？
                          （公主清澈的大眼睛望着你，带着全然的信任和期盼。）
                          """;
            HistoryDialogues = "";
            PlotDirections = "3 - 前往太学听晏笑的课";
            Characters = string.Join("\n",
                Enumerable.Repeat(new { Name = "测试角色", Personality = "测试性格测试性格测试性格测试性格测试性格测试性格测试性格测试性格测试性格" }, 5)
                    .Select(c => $"{c.Name}：{c.Personality}"));
            InternalThoughts = string.IsNullOrWhiteSpace("") ? "You have no previous thoughts." : "测试内部思想";
        }

        public int MaxTurns { get; set; }

        public int CurrentTurn { get; set; }

        public string NodeSummary { get; init; }

        public string HistoryDialogues { get; init; }

        public string Characters { get; init; }

        public string PlotDirections { get; init; }

        public string InternalThoughts { get; init; }
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
    public void GenerateFullPrompt_Success_Test()
    {
        var template = _serviceProvider.GetRequiredService<PromptAssemblerFactory>()
            .Create()
            .WithMessageTemplate<NarrationTemplate>(PromptMessage.Roles.System, "debug")
            .WithResponseFormat<TestModel>();

        var e = template.FillMessage(new NarrationTemplate());
        var prompt = e.GetPrompt();
        foreach (var p in prompt)
        {
            testOutputHelper.WriteLine(p.ToString());
        }
    }
}