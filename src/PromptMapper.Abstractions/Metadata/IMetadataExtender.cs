using System.Reflection;

namespace PromptMapper.Abstractions.Metadata
{
    public interface IMetadataExtender
    {
        int Order { get; }
        void ExtendPropertyMetadata(PropertyInfo property, ModelPropertyMetadata metadata);
    }
}