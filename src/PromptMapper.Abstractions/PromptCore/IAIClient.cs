using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromptMapper.Abstractions.PromptCore
{
    public interface IAIClient
    {
        Task<string> SendPromptAsync(IReadOnlyList<PromptMessage> messages);
    }
}