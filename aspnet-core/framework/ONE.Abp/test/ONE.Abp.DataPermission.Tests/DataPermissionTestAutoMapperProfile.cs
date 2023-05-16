using AutoMapper;
using ONE.Abp.DataPermission.Tests.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONE.Abp.DataPermission.Tests
{
   
    public class DataPermissionTestAutoMapperProfile : Profile
    {
        public DataPermissionTestAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Customer, CustomerDto>().ForMember(d => d.MapAddress, opt => opt.MapFrom(s => s.Address));
            CreateMap<Address, AddressDto>().ForMember(d => d.MapCity, opt => opt.MapFrom(s => s.City));
            CreateMap<Team, TeamDto>();
        }
    }
}
