using Volo.Abp.Data;

namespace ONE.Abp.DataPermission.Rules
{
    public class IdName<Tkey>
    {
        public Tkey Id { get; set; }

        public string Name { get; set; }
    }

    public class IdNameWithExtraProperties<Tkey>:IHasExtraProperties
    {
        public Tkey Id { get; set; }

        public string Name { get; set; }

        public ExtraPropertyDictionary ExtraProperties { get; set; }

    }
}
