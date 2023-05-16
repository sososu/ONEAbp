namespace Volo.Abp.OpenIddict
{
    public class OpenIddictScopeCreateInput
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Resources { get; set; }
    }

    public class OpenIddictScopeUpdateInput
    {

        public string DisplayName { get; set; }

        public string Resources { get; set; }
    }
}
