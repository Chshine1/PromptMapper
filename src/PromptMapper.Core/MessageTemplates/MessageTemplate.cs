using System.Text;
using System.Text.RegularExpressions;
using PromptMapper.Abstractions.MessageTemplates;

namespace PromptMapper.Core.MessageTemplates;

public class MessageTemplate<TTemplate> : IMessageTemplate<TTemplate> where TTemplate : class
{
    private readonly string _template;
    private readonly Dictionary<string, Func<TTemplate, string>> _parameters;
    
    public MessageTemplate(string template)
    {
        _template = template;
        _parameters = ExtractParameters(template);
    }

    public string Render(TTemplate templateInstance)
    {
        var builder = new StringBuilder(_template);
        foreach (var param in _parameters)
        {
            var value = param.Value(templateInstance);
            builder.Replace(param.Key, value);
        }
        return builder.ToString();
    }

    private static Dictionary<string, Func<TTemplate, string>> ExtractParameters(string template)
    {
        var parameters = new Dictionary<string, Func<TTemplate, string>>();
        var matches = Regex.Matches(template, @"\{(\w+)\}");
        
        foreach (Match match in matches)
        {
            var paramName = match.Groups[1].Value;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string Func(TTemplate obj) => typeof(TTemplate).GetProperty(paramName).GetValue(obj).ToString() ?? "";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            parameters[match.Groups[0].Value] = (Func<TTemplate, string>)Func;
        }
        
        return parameters;
    }
}