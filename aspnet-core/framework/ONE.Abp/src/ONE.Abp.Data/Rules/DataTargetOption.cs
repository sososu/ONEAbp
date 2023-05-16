using ONE.Abp.Shared.Rules;
using  ONE.Abp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace ONE.Abp.Data.Rules
{
    public class DataTargetOption
    {
        private List<DataTargetInfo> _dataTargetInfos;

        public void Add<T>(bool needUpdateShadowProperty=false,string displayName = null, string description = null) where T : IEntity
        {
            var dataTarget = _dataTargetInfos.FirstOrDefault(t => t.DataTargetType == typeof(T));
            if (dataTarget != null)
            {
                dataTarget.NeedUpdateShadowProperty= needUpdateShadowProperty;
                dataTarget.DisplayName= displayName ?? typeof(T).DisplayName();
                dataTarget.Description= description ?? typeof(T).DisplayDescription();
                return;
            }
               
            _dataTargetInfos.Add(new DataTargetInfo { DataTargetType = typeof(T), NeedUpdateShadowProperty= needUpdateShadowProperty, DisplayName = displayName ?? typeof(T).DisplayName(), Description = description ?? typeof(T).DisplayDescription() });
        }

        public void Remove<T>() where T : IEntity
        {
            _dataTargetInfos.RemoveAll(t => t.DataTargetType == typeof(T));
        }

        public IReadOnlyCollection<DataTargetInfo> GetInfos()
        {
            return _dataTargetInfos.ToImmutableList();
        }


        public IList<Type> GetNeedUpdateShadowPropertyType()
        {
            return _dataTargetInfos.Where(d=>d.NeedUpdateShadowProperty).Select(d=>d.DataTargetType).ToList();
        }

        public DataTargetOption() { _dataTargetInfos = new List<DataTargetInfo>(); }
    }
}
