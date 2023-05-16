using JetBrains.Annotations;
using System;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Volo.Abp;

namespace ONE.Abp.DataPermission.Rules
{
    public class UserDataRuleManager : RuleDomainService, IUserDataRuleManager
    {

        protected IUserDataRuleRepository UserDataRuleRepository { get; }
        public UserDataRuleManager(IUserDataRuleRepository sysAppRepository)
        {
            UserDataRuleRepository = sysAppRepository;
        }

        public async Task ChangeeUserDataIdAsync([NotNull] UserDataRule userDataRule, [NotNull] string dataTarget, [NotNull] Guid userRuleId, [NotNull] Guid dataRuleId)
        {
            Check.NotNull(dataTarget, nameof(dataTarget));
            Check.NotNull(userRuleId, nameof(userRuleId));
            Check.NotNull(dataRuleId, nameof(dataRuleId));

            await ValidateUserDataIdAsync(dataTarget, userRuleId, dataRuleId,userDataRule.Id);
            userDataRule.Change(dataTarget, userRuleId, dataRuleId);
        }

        public async Task<UserDataRule> CreateAsync([NotNull] string dataTarget, [NotNull] Guid userRuleId, [NotNull] Guid dataRuleId)
        {
            Check.NotNull(dataTarget, nameof(dataTarget));
            Check.NotNull(userRuleId, nameof(userRuleId));
            Check.NotNull(dataRuleId, nameof(dataRuleId));

            await ValidateUserDataIdAsync(dataTarget, userRuleId, dataRuleId);
            return new UserDataRule(GuidGenerator.Create(), dataTarget, userRuleId, dataRuleId);
        }

        protected virtual async Task ValidateUserDataIdAsync([NotNull] string dataTarget, [NotNull] Guid userRuleId, [NotNull] Guid dataRuleId, Guid? expectedId = null)
        {
            var isConsistent = await UserDataRuleRepository.CheckDataTargetNameConsistent(dataTarget, dataRuleId);
            if (!isConsistent)
                throw new BusinessException(DataPermissionErrorCodes.DataTargetNameInconsistent);

            var userDataRule = await UserDataRuleRepository.FindByUserDataIdAsync(dataTarget,userRuleId,dataRuleId);
            if (userDataRule != null && userDataRule.Id != expectedId)
            {
                throw new BusinessException(DataPermissionErrorCodes.DuplicateUserDataRule)
                    .WithData("dataTarget", dataTarget)
                    .WithData("userRuleId", userRuleId)
                    .WithData("dataRuleId", dataRuleId);
            }
        }
    }
}
