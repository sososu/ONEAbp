using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Account;

public class ProfileDto : ExtensibleObject, IHasConcurrencyStamp
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string PhoneNumber { get; set; }

    public bool IsExternal { get; set; }

    public bool HasPassword { get; set; }

    public string ConcurrencyStamp { get; set; }

    /// <summary>
    /// user Avatar
    /// </summary>
    [CanBeNull]
    public virtual string Avatar { get; set; }


    public virtual Sex Sex { get; set; }
}
