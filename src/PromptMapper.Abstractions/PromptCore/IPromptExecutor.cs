using System.Threading.Tasks;

namespace PromptMapper.Abstractions.PromptCore
{
    public interface IPromptExecutor<TResponse> where TResponse : class
    {
        Task<TResponse> ExecuteAsync();
    }
}