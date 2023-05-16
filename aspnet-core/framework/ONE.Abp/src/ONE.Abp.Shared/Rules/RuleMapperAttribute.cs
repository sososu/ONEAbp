using System;

namespace ONE.Abp.Shared.Rules
{
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class RuleMapperAttribute:Attribute
    {
        public string SourceProperty { get; set; }

        public RuleMapperAttribute(string sourceProperty) { SourceProperty = sourceProperty; }
    }
}
