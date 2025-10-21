using System;

namespace PromptMapper.Abstractions.Interfaces.ResponseFormat
{
    public interface IResponseMessageCompiler
    {
        string Compile(Type type);
        
        IResponseMessage CompileA(Type type);
    }
}