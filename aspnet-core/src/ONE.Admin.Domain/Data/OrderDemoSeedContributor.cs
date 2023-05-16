using ONE.Admin.RuleDemo;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ONE.Admin.Data
{
    public class OrderDemoSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<OrderDemo> _repository;
        public OrderDemoSeedContributor(IRepository<OrderDemo> repository)
        {
            _repository = repository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await _repository.DeleteAsync(r => true);


            var demos = new List<OrderDemo>() {
       new OrderDemo("电视", 9999, "18200000002", "天津市兴安盟市永川王街t座 506488", "李四"),
       new OrderDemo("电视", 9999, "18200000003", "上海市兴安盟市沈河王街t座 506489", "王五"),
       new OrderDemo("冰箱", 18652, "18200000004", "广东省兴安盟市海陵王街t座 506490", "赵六"),
       new OrderDemo("空调", 8555, "18200000005", "河南省兴安盟市兴山王街t座 506491", "孙七"),
       new OrderDemo( "电视", 9999, "18200000006", "湖北省兴安盟市南湖王街t座 506492", "周八"),
       new OrderDemo( "电视", 9999, "18200000007", "黑龙江省兴安盟市兴山王街t座 506493", "吴九"),
       new OrderDemo("空调", 8555, "18200000008", "贵州省兴安盟市南湖王街t座 506494", "郑十"),
       new OrderDemo( "电视",9999, "18200000001", "北京市兴安盟市普陀王街t座 506487", "张三"),
       new OrderDemo("电视", 9999, "18200000009", "吉林省兴安盟市永川王街t座 506495", "赵十"),
       new OrderDemo("电视", 9999, "182000000010", "江苏省兴安盟市南湖王街t座 506496", "孙十"),
       new OrderDemo("空调", 8555, "182000000011", "浙江省兴安盟市永川王街t座 506497", "周十"),
       new OrderDemo("电视", 9999, "182000000012", "陕西省兴安盟市永川王街t座 506498", "吴十"),
       new OrderDemo("冰箱", 18652, "182000000013", "甘肃省兴安盟市永川王街t座 OrderDemo", "郑十"),
       new OrderDemo( "冰箱", 18652, "182000000014", "青海省兴安盟市永川王街t座 506490", "赵十"),
       new OrderDemo("冰箱", 18652, "182000000015", "西藏自治区兴安盟市永川王街t座 506491", "孙七")};


            await _repository.InsertManyAsync(demos);

        }
    }
}
