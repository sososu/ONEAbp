namespace ONE.Abp.Pagination
{
    public class ServiceContainer
    {

        public static IServiceProvider ServiceProvider { get; private set; }


        public static void Set(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }
    }
}
