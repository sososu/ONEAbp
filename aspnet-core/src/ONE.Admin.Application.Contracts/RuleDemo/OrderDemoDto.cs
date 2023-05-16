using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Volo.Abp.Application.Dtos;

namespace ONE.Admin.RuleDemo
{
    public class OrderDemoDto: ExtensibleAuditedEntityDto<Guid>
    {
        public string ProductName { get; set; }

      
        public decimal Amount { get; set; }


       
        public string Mobile { get; set; }


       
        public string Address { get; set; }

       
        public string Addressee { get; set; }

        public string OrganizationCode { get;  set; }
    }
}
