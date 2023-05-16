using Microsoft.EntityFrameworkCore;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Pagination;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.DataPermission.Rules
{
    public class DataTargetAppService : ApplicationService, IDataTargetAppService
    {
        private readonly IRepository<DataTarget> _dataTargetRepository;
        public DataTargetAppService(IRepository<DataTarget> dataTargetRepository)
        {
            _dataTargetRepository = dataTargetRepository;
        }

        public async Task<PagedResult<DataTargetDto>> QueryAsync(DataTargetQueryInput input)
        {
           return await (await _dataTargetRepository.WithDetailsAsync()).Include(d => d.Fields).ToPagedResultAsync<DataTarget, DataTargetDto>(input);
        }


        public async Task<ListResultDto<DataTargetDto>> GetListAsync()
        {
            var dataTargets= await _dataTargetRepository.GetListAsync();
            return new ListResultDto<DataTargetDto>(ObjectMapper.Map<List<DataTarget>, List<DataTargetDto>>(dataTargets));
        }

        public async Task<DataTargetDto> GetAsync(string name)
        {
            var dataTarget = await (await _dataTargetRepository.WithDetailsAsync()).Include(d => d.Fields).Where(r=>r.Name==name).FirstOrDefaultAsync();
            if (dataTarget == null)
                return null;
            return ObjectMapper.Map<DataTarget, DataTargetDto>(dataTarget);
        }
    }
}
