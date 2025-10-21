using System;

namespace PromptMapper.Abstractions.Metadata
{
    public interface IMetadataExtractor
    {
        ModelMetadata GetMetadata(Type type);
    }
}