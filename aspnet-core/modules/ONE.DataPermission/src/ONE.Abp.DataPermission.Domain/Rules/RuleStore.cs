using Microsoft.Extensions.Caching.Distributed;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace ONE.Abp.DataPermission.Rules
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IRuleStore))]
    public class RuleStore : IRuleStore, ITransientDependency
    {
        protected IDistributedCache<DataRuleResult> DataRuleCache { get; }
        protected IDistributedCache<UserRuleResult> UserRuleCache { get; }
        protected IDistributedCache<List<UserDataRuleResult>> UserDataRuleCache { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }


        protected IUserDataRuleRepository UserDataRuleRepository { get; }
        protected IRepository<DataRule> DataRuleRepository { get; }
        protected IRepository<UserRule> UserRuleRepository { get; }
        protected IObjectMapper ObjectMapper { get; }

        const int TIMEOUTHOUR = 24; //TODO:可配置？
        public RuleStore(IDistributedCache<DataRuleResult> dataRuleCache, IDistributedCache<UserRuleResult> userRuleCache, IDistributedCache<List<UserDataRuleResult>> userDataRuleCache, IUnitOfWorkManager unitOfWorkManager, IUserDataRuleRepository userDataRuleRepository, IRepository<DataRule> dataRuleRepository, IRepository<UserRule> userRuleRepository, IObjectMapper objectMapper)
        {
            DataRuleCache = dataRuleCache;
            UserRuleCache = userRuleCache;
            UserDataRuleCache = userDataRuleCache;
            UnitOfWorkManager = unitOfWorkManager;
            UserDataRuleRepository = userDataRuleRepository;
            DataRuleRepository = dataRuleRepository;
            UserRuleRepository = userRuleRepository;
            ObjectMapper = objectMapper;
        }

        public async Task<List<UserDataRuleResult>> GetUserDataRulesAsync(string dataTarget, CancellationToken cancellationToken = default)
        {
            var key = string.Format(RuleCacheKeys.UserDataRuleKey, dataTarget);

            var data = await UserDataRuleCache.GetAsync(key, null, false, cancellationToken);

            if (data != null)
                return data;

            var result= await UserDataRuleRepository.GetListAsync(dataTarget,cancellationToken);

            data = ObjectMapper.Map<List<UserDataRule>, List<UserDataRuleResult>>(result);
            await UserDataRuleCache.SetAsync(key, data, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = new TimeSpan(TIMEOUTHOUR, 0, 0) }, null, false, cancellationToken);
            return data;
        }
        public async Task<UserRuleResult> GetUserRuleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var key = string.Format(RuleCacheKeys.UserRuleKey, id);

            var data = await UserRuleCache.GetAsync(key, null, false, cancellationToken);

            if (data != null)
                return data;


            var result = await UserRuleRepository.FindAsync(u=>u.Id==id,true,cancellationToken);
            if(result==null)
                data = null;
            else
                data = ObjectMapper.Map<UserRule, UserRuleResult>(result);

            await UserRuleCache.SetAsync(key, data, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = new TimeSpan(TIMEOUTHOUR, 0, 0) }, null, false, cancellationToken);

            return data;
        }

        public async Task<DataRuleResult> GetDataRuleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var key = string.Format(RuleCacheKeys.DataRuleKey, id);

            var data = await DataRuleCache.GetAsync(key,null,false, cancellationToken);

            if (data != null)
                return data;


            var result = await DataRuleRepository.FindAsync(u => u.Id == id,true,cancellationToken);
            if (result == null)
                data = null;
            else
                data = ObjectMapper.Map<DataRule, DataRuleResult>(result);

            await DataRuleCache.SetAsync(key, data, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = new TimeSpan(TIMEOUTHOUR, 0, 0) },null,false,cancellationToken);

            return data;
        }

    }
}
