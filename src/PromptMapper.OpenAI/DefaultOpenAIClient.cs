using OpenAI.Chat;
using PromptMapper.Abstractions;
using PromptMapper.Abstractions.PromptCore;

namespace PromptMapper.OpenAI;

public class DefaultOpenAIClient : IAIClient
{
    private readonly ChatClient _client;

    public DefaultOpenAIClient(ChatClient client)
    {
        _client = client;
    }

    public async Task<string> SendPromptAsync(IReadOnlyList<PromptMessage> messages)
    {
        var m = new ChatMessage[messages.Count];
        for (var i = 0; i < messages.Count; i++)
        {
            var message = messages[i];
            m[i] = message.Role switch
            {
                PromptMessage.Roles.System => new SystemChatMessage(message.Content),
                PromptMessage.Roles.User => new UserChatMessage(message.Content),
                PromptMessage.Roles.Assistant => new AssistantChatMessage(message.Content),
                _ => throw new NotImplementedException()
            };
        }

        var chat = await _client.CompleteChatAsync(m);
        return chat.Value.Content[0].Text;
    }
}