using PromptMapper.Abstractions.MessageTemplates;

namespace PromptMapper.Core.Common;

public interface IMessageEntry
{
    bool IsStatic { get; }
    bool IsRendered { get; }
    Type? TemplateType { get; }
    
    void Render(object? instance);
    string RenderedMessage { get; }

    string Role { get; }
}

public abstract class MessageEntry : IMessageEntry
{
    public bool IsStatic { get; }
    public bool IsRendered { get; protected set; }
    public abstract Type? TemplateType { get; }

    public virtual void Render(object? instance) { }

    public abstract string RenderedMessage { get; }
    public string Role { get; }

    protected MessageEntry(bool isStatic, string role)
    {
        IsStatic = isStatic;
        Role = role;
    }
}

public class StaticMessageEntry : MessageEntry
{
    public override Type? TemplateType => null;
    
    public override string RenderedMessage { get; }

    public StaticMessageEntry(string role, string message) : base(true, role)
    {
        RenderedMessage = message;
        IsRendered = true;
    }
}

public class DynamicMessageEntry<TTemplate> : MessageEntry where TTemplate : class
{
    private readonly IMessageTemplate<TTemplate> _template;
    private string _message;

    public override Type TemplateType => typeof(TTemplate);

    public override void Render(object? instance)
    {
        if (instance is not TTemplate typedInstance) throw new InvalidOperationException();
        _message = _template.Render(typedInstance);
        IsRendered = true;
    }

    public override string RenderedMessage => IsRendered ? _message : throw new InvalidOperationException();

    public DynamicMessageEntry(string role, IMessageTemplate<TTemplate> template) : base(false, role)
    {
        _message = string.Empty;
        _template = template;
    }
}