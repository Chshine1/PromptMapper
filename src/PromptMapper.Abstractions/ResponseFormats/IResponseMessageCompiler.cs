using System;

namespace PromptMapper.Abstractions.ResponseFormats
{
    public interface IResponseMessageCompiler
    {
        string Compile(Type type);
    }
}