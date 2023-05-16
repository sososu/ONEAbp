using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ONE.Abp.DataPermission.Tests.Datas
{
    public class CustomerData
    {
        public static IList<Customer> Customers => new List<Customer> { CustomerA, CustomerB, CustomerC };

        public static Customer CustomerA = new Customer(Guid.NewGuid())
        {
            Name = "A",
            Address = new Address("中国", "深圳", "南山区", "68255", "宝安区新安一路69号"),
            Mobile = "12546584541",
            Teams = new List<Team>
                      {
                          new Team {Name="Ta",Mobile="1551552"}
                      },
            OrganizationCode = "V",
            CreatorId = Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0d")
        };


        public static Customer CustomerB =new Customer(Guid.NewGuid())
        {
            Name = "B",
                    Address = new Address("中国", "深圳", "南山区", "68255", "宝安区新安一路66号"),
                    Mobile = "12546584541",
                    Teams = new List<Team>
                    {
                        new Team { Name = "Tb" ,Mobile="1551552"}
                    },
                    OrganizationCode = "V",
                    CreatorId = Guid.NewGuid()
                };


        public static Customer CustomerC = new Customer(Guid.NewGuid())
        {
            Name = "C",
            Address = new Address("中国", "北京", "一环区", "68255", "长安区新华大道44号"),
            Mobile = "12546584541",
            Teams = new List<Team>
                    {
                        new Team { Name = "Tc",Mobile="1551552" }
                    },
            OrganizationCode = "C",
            CreatorId = Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0d")
        };
    }
}
