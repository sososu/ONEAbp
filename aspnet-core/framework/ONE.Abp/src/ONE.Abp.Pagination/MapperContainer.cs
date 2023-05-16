using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONE.Abp.Pagination
{
    public class MapperContainer
    {
        public static IMapper Mapper = MapperProviderContainer.Configuration.CreateMapper();
    }
}
