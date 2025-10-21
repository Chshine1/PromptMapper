using Microsoft.Extensions.DependencyInjection;
using PromptMapper.Abstractions.Interfaces;
using PromptMapper.Abstractions.Interfaces.MessageTemplate;
using PromptMapper.Abstractions.Interfaces.ResponseFormat;
using PromptMapper.Abstractions.Metadata;
using PromptMapper.Core.Implementations;
using PromptMapper.Core.MessageTemplate;
using PromptMapper.Core.Metadata;
using PromptMapper.SystemTextJson;

namespace PromptMapper.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateCore(this IServiceCollection services)
    {
        services.AddSingleton<IMetadataExtractor, MetadataExtractor>();
        services.AddSingleton<IJsonSchemaGenerator, JsonSchemaGenerator>();
        
        services.AddSingleton<IMessageTemplateCompiler, MessageTemplateCompiler>();
        services.AddSingleton<IResponseMessageCompiler, ResponseMessageCompiler>();
            
        services.AddTransient<IPromptAssembler, PromptAssembler>();
            
        return services;
    }
}