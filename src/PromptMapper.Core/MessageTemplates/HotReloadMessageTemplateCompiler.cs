using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using PromptMapper.Abstractions.MessageTemplates;
using PromptMapper.Abstractions.Metadata;
using PromptMapper.Core.PromptCore.Configurations;

namespace PromptMapper.Core.MessageTemplates;

public class HotReloadMessageTemplateCompiler : IMessageTemplateCompiler
{
    private readonly IMetadataExtractor _metadataExtractor;

    private readonly string _templateDirectory;
    private readonly ConcurrentDictionary<string, string> _cache;
    private readonly FileSystemWatcher _watcher;
    
    public HotReloadMessageTemplateCompiler(IOptions<PromptOptions> options, IMetadataExtractor metadataExtractor)
    {
        _cache = new ConcurrentDictionary<string, string>();
        _metadataExtractor = metadataExtractor;
        
        _templateDirectory = options.Value.TemplateDirectory;
        _watcher = new FileSystemWatcher(_templateDirectory, "*.template");
        _watcher.Changed += OnTemplateFileChanged;
        _watcher.EnableRaisingEvents = true;
    }
    
    public IMessageTemplate<TTemplate> Compile<TTemplate>(string? key) where TTemplate : class
    {
        var metadata = _metadataExtractor.GetMetadata(typeof(TTemplate));
        if (!metadata.IsMessageTemplate
            || string.IsNullOrWhiteSpace(metadata.TemplateName)
            || key != null && !metadata.Keys.Contains(key))
        {
            throw new InvalidOperationException($"Template '{typeof(TTemplate).Name}' is not a request template");
        }

        var fullname = metadata.TemplateName + (key == null ? string.Empty : $".{key}");
        if (_cache.TryGetValue(fullname, out var cachedTemplate))
        {
            return new MessageTemplate<TTemplate>(cachedTemplate);
        }
        
        var path = Path.Combine(_templateDirectory, fullname + ".template");
        var content = File.ReadAllText(path);
        _cache[fullname] = content;
        return new MessageTemplate<TTemplate>(content);
    }
    
    private void OnTemplateFileChanged(object sender, FileSystemEventArgs e)
    {
        var fullname = Path.GetFileNameWithoutExtension(e.Name)?.Replace(".template", "");
        if (string.IsNullOrWhiteSpace(fullname)) throw new InvalidOperationException($"Template '{fullname}' is not a request template");

        _cache[fullname] = File.ReadAllText(e.FullPath);
    }
}