using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace ONE.Abp.SysResource.SaleVersions
{
    /// <summary>
    /// 销售版本
    /// </summary>
    public class SaleVersion : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; protected set; }


        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price { get; protected set; }

        /// <summary>
        /// 描述
        /// </summary>
        [CanBeNull]
        public virtual string Description { get; protected set; }

        public virtual ICollection<SaleVersionMenu> Menus { get; protected set; }

        protected SaleVersion() { }


        public SaleVersion(Guid id, string name)
        {
            Id = id;
            Name = name;
            Menus=new Collection<SaleVersionMenu>();
        }

        public void SetName([NotNull]string name)
        {
            Check.NotNull(name, nameof(name));
            Name = name; 
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }


        public virtual void AddMenu(Guid appId,string menuCode)
        {
            Check.NotNull(appId, nameof(appId));
            Check.NotNull(menuCode, nameof(menuCode));

            if (IsInMenu(menuCode))
            {
                return;
            }

            Menus.Add(new SaleVersionMenu(Id, appId, menuCode));
        }

        public virtual void RemoveMenu(string menuCode)
        {
            Check.NotNull(menuCode, nameof(menuCode));

            if (!IsInMenu(menuCode))
            {
                return;
            }

            Menus.RemoveAll(r => r.MenuCode == menuCode);
        }

        public virtual void RemoveAllMenu()
        {
            Menus.Clear();
        }

        public virtual bool IsInMenu(string menuCode)
        {
            Check.NotNull(menuCode, nameof(menuCode));

            return Menus.Any(r => r.MenuCode == menuCode);
        }

    }
}
