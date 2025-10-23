using System.Collections.Generic;

namespace PromptMapper.Abstractions.PromptCore
{
    public interface IPromptEngine<TResponse> where TResponse : class
    {
        /// <summary>
        /// Fill the instance data into a message.
        /// </summary>
        /// <param name="templateInstance">The instance data filled into the message.</param>
        /// <param name="key">Identify the message when more than one are registered from the same template type.</param>
        /// <typeparam name="TTemplate">The template type which needs filling.</typeparam>
        /// <returns>The new filled conversation.</returns>
        IPromptEngine<TResponse> FillMessage<TTemplate>(TTemplate templateInstance, string? key = null) where TTemplate : class;

        /// <summary>
        /// Check whether the whole prompt is filled.
        /// </summary>
        bool IsFilled();

        IReadOnlyList<PromptMessage> GetPrompt();
    }
}