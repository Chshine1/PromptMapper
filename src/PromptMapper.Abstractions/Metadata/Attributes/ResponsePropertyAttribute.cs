using System;

namespace PromptMapper.Abstractions.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ResponsePropertyAttribute : Attribute
    {
        private string _description;
        private string _constraints;

        public string Description
        {
            get => _description;
            set => _description = value ?? string.Empty;
        }

        public string Constraints
        {
            get => _constraints;
            set => _constraints = value ?? string.Empty;
        }
    }
}