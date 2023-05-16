using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace ONE.Admin.RuleDemo
{
    public class OrderDemo:AuditedAggregateRoot<Guid>,IOrganizationCode
    {
        [Display(Name = "产品名称", Description = "字符串类,200字符内")]
        public string ProductName { get; protected set; }

        [Display(Name ="金额",Description = "数字类型")]
        public decimal Amount { get; protected set; }


        [Display(Name = "手机号", Description = "字符串类型,11位")]
        public string Mobile { get; protected set; }


        [Display(Name = "地址", Description = "字符串类型,400字符内")]
        public string Address { get; protected set; }

        [Display(Name = "收件人", Description = "字符串类型,64字符内")]
        public string Addressee { get; protected set; }


        [Display(Name = "部门编码", Description = "字符串类型")]
        public string OrganizationCode { get; set; }

        public OrderDemo(string productName, decimal amount, string mobile, string address, string addressee)
        {
            ProductName = productName;
            Amount = amount;
            Mobile = mobile;
            Address = address;
            Addressee = addressee;
        }

        protected OrderDemo()
        {

        }
    }
}
