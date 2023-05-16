using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.TenantManagement;

public class Tenant : FullAuditedAggregateRoot<Guid>
{
    public virtual string Name { get; protected set; }

    [CanBeNull]
    public virtual string Contact { get; protected set; }

    [CanBeNull]
    public virtual string ContactWay { get; protected set; }

    public virtual DateTime? ExpirationDate { get; protected set; }

    public virtual bool IsActive { get; protected set; }

    public virtual Guid? SaleVersionId { get; protected set; }

    public virtual List<TenantConnectionString> ConnectionStrings { get; protected set; }

    protected Tenant()
    {

    }

    protected internal Tenant(Guid id, [NotNull] string name)
        : base(id)
    {
        SetName(name);
        SetIsActive(true);
        ConnectionStrings = new List<TenantConnectionString>();
    }

    [CanBeNull]
    public virtual string FindDefaultConnectionString()
    {
        return FindConnectionString(Data.ConnectionStrings.DefaultConnectionStringName);
    }

    [CanBeNull]
    public virtual string FindConnectionString(string name)
    {
        return ConnectionStrings.FirstOrDefault(c => c.Name == name)?.Value;
    }

    public virtual void SetDefaultConnectionString(string connectionString)
    {
        SetConnectionString(Data.ConnectionStrings.DefaultConnectionStringName, connectionString);
    }

    public virtual void SetConnectionString(string name, string connectionString)
    {
        var tenantConnectionString = ConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            tenantConnectionString.SetValue(connectionString);
        }
        else
        {
            ConnectionStrings.Add(new TenantConnectionString(Id, name, connectionString));
        }
    }

    public virtual void RemoveDefaultConnectionString()
    {
        RemoveConnectionString(Data.ConnectionStrings.DefaultConnectionStringName);
    }

    public virtual void RemoveConnectionString(string name)
    {
        var tenantConnectionString = ConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            ConnectionStrings.Remove(tenantConnectionString);
        }
    }

    public virtual void RemoveAllConnectionString()
    {
        ConnectionStrings.Clear();
    }

    public virtual void SetContact(string contact, string contactWay)
    {
        Contact = contact; ContactWay = contactWay;
    }

    public virtual void SetIsActive(bool isActive)
    {
        IsActive= isActive;
    }

    public virtual void SetSaleVersionId([NotNull] Guid saleVersoionId)
    {
        SaleVersionId = Check.NotNull(saleVersoionId, nameof(saleVersoionId));
    }

    public virtual void SetExpirationDate([NotNull] DateTime expirationDate)
    {
        ExpirationDate = Check.NotNull(expirationDate, nameof(expirationDate));
    }

    protected internal virtual void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConsts.MaxNameLength);
    }
}
