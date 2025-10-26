using Microsoft.Extensions.DependencyInjection;
using PromptMapper.Abstractions.MessageTemplates;
using PromptMapper.Abstractions.Metadata;
using PromptMapper.Abstractions.PromptCore;
using PromptMapper.Abstractions.ResponseFormats;
using PromptMapper.Core.JsonSchema;
using PromptMapper.Core.MessageTemplates;
using PromptMapper.Core.Metadata;
using PromptMapper.Core.PromptCore;
using PromptMapper.Core.ResponseFormats;
using PromptMapper.SystemTextJson;

namespace PromptMapper.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateCore(this IServiceCollection services)
    {
        services.AddSingleton<IMetadataExtractor, MetadataExtractor>();
        services.AddSingleton<IJsonSchemaGenerator, JsonSchemaGenerator>();
        
        services.AddSingleton<IMessageTemplateCompiler, HotReloadMessageTemplateCompiler>();
        services.AddSingleton<IResponseMessageCompiler, ResponseMessageCompiler>();

        services.AddSingleton<PromptAssemblerFactory>();
            
        return services;
    }
}