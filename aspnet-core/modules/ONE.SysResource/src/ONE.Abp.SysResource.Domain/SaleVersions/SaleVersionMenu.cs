using System;
using Volo.Abp.Domain.Entities;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionMenu : Entity
    {
        public virtual Guid SaleVersonId { get;protected set; }

        public virtual Guid AppId { get; protected set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public virtual string MenuCode { get; protected set; }

        protected SaleVersionMenu()
        {

        }

        public SaleVersionMenu(Guid saleVersionId,Guid appId, string menuCode)
        {
            SaleVersonId = saleVersionId;
            AppId = appId;
            MenuCode = menuCode;
        }

        public override object[] GetKeys()
        {
            return new object[] { SaleVersonId,MenuCode };
        }
    }
}
