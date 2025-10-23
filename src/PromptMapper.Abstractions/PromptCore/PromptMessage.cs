namespace PromptMapper.Abstractions.PromptCore
{
    public class PromptMessage
    {
        public string Role { get; }
        public string Content { get; }
    
        public PromptMessage(string role, string content)
        {
            Role = role;
            Content = content;
        }
        
        public static class Roles
        {
            public const string System = "system";
            public const string User = "user";
            public const string Assistant = "assistant";
        }

        public override string ToString()
        {
            return $"{{ role: \"{Role}\", content: \"{Content}\" }}";
        }
    }
}