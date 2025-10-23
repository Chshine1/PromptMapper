namespace PromptMapper.Abstractions.PromptCore
{
    public interface IPromptAssembler
    {
        /// <summary>
        /// Add a message template to the current conversation template.
        /// </summary>
        /// <param name="role">Message role.</param>
        /// <param name="key">An optional key for the template, distinct templates registered from the same type.</param>
        /// <typeparam name="TTemplate">The type for generating the template, labeled by MessageTemplateAttribute.</typeparam>
        /// <returns>The new conversation template.</returns>
        IPromptAssembler WithMessageTemplate<TTemplate>(string role, string? key = null) where TTemplate : class;

        /// <summary>
        /// Add a fixed message to the current conversation template.
        /// </summary>
        /// <param name="role">Message role.</param>
        /// <param name="message">Message content.</param>
        /// <returns>The new conversation template.</returns>
        IPromptAssembler WithStaticMessage(string role, string message);

        /// <summary>
        /// Assigns to the template a response type, then compiles to a conversation.
        /// </summary>
        /// <typeparam name="TResponse">The type for accepting the response, labeled by ResponseSchemaAttribute.</typeparam>
        /// <returns>The conversation to fill and execute prompts.</returns>
        IPromptEngine<TResponse> WithResponseFormat<TResponse>() where TResponse : class;
    }
}