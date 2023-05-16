using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;
using ONE.Abp.Data.Rules;
using JetBrains.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ONE.Abp.Shared.Rules;

namespace ONE.Abp.DataPermission.Tests.Datas
{
    public class Customer : AuditedAggregateRoot<Guid>,IOrganizationCode
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public Address Address { get; set; }

        [CanBeNull]
        public string? OrganizationCode { get; set; }

        public List<Team> Teams { get; set; }

        protected Customer() { }

        public Customer(Guid id) { Id = id; }
    }


    public class Team : Entity
    {
        public string Name { get; set; }
        [CanBeNull]
        public string Mobile { get; set; }

        public Guid CustomerId { get; set; }
        public override object[] GetKeys()
        {
            return new object[] { CustomerId, Name };
        }

    }

    public class Address : ValueObject
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Detial { get; set; }


        public Address(string country, string city, string region, string postalCode, string detial)
        {
            Country = country;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Detial = detial;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Country;
            yield return City;
            yield return Region;
            yield return PostalCode;
            yield return Detial;
        }
    }

    public class CustomerDto : ExtensibleAuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        [RuleMapper("Address")]
        public AddressDto MapAddress { get; set; }

        public string OrganizationCode { get; set; }

        public List<TeamDto> Teams { get; set; }

    }
    public class AddressDto
    {
        public string Country { get; set; }

        [RuleMapper("City")]
        public string MapCity { get; set; }
        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Detial { get; set; }
    }


    public class TeamDto
    {
        public string Name { get; set; }
        public string Mobile { get; set; }

        public Guid CustomerId { get; set; }
    }
}
