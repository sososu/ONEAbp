using JetBrains.Annotations;

namespace ONE.Abp.Shared.Rules
{
    public interface IOrganizationCode:IShadowProperty
    {
        [CanBeNull]
        string? OrganizationCode { get; set; }    
    }
}
