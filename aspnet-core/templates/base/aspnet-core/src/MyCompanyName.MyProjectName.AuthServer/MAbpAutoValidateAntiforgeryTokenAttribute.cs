using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MAbpAutoValidateAntiforgeryTokenAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        /// <summary>
        /// Gets the order value for determining the order of execution of filters. Filters execute in
        /// ascending numeric value of the <see cref="Order"/> property.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Filters are executed in a sequence determined by an ascending sort of the <see cref="Order"/> property.
        /// </para>
        /// <para>
        /// The default Order for this attribute is 1000 because it must run after any filter which does authentication
        /// or login in order to allow them to behave as expected (ie Unauthenticated or Redirect instead of 400).
        /// </para>
        /// <para>
        /// Look at <see cref="IOrderedFilter.Order"/> for more detailed info.
        /// </para>
        /// </remarks>
        public int Order { get; set; } = 1000;

        /// <inheritdoc />
        public bool IsReusable => true;

        /// <inheritdoc />
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<MAbpAutoValidateAntiforgeryTokenAuthorizationFilter>();
        }
    }

    public class MAbpAutoValidateAntiforgeryTokenAuthorizationFilter : AbpValidateAntiforgeryTokenAuthorizationFilter, ITransientDependency
    {
        private readonly AbpAntiForgeryOptions _options;

        public MAbpAutoValidateAntiforgeryTokenAuthorizationFilter(
            IAntiforgery antiforgery,
            AbpAntiForgeryCookieNameProvider antiForgeryCookieNameProvider,
            IOptions<AbpAntiForgeryOptions> options,
            ILogger<AbpValidateAntiforgeryTokenAuthorizationFilter> logger)
            : base(
                antiforgery,
                antiForgeryCookieNameProvider,
                logger)
        {
            _options = options.Value;
        }

        protected override bool ShouldValidate(AuthorizationFilterContext context)
        {
            if (!_options.AutoValidate)
            {
                return false;
            }

            if (context.ActionDescriptor.IsControllerAction())
            {
                var controllerType = context.ActionDescriptor
                    .AsControllerActionDescriptor()
                    .ControllerTypeInfo
                    .AsType();

                if (!_options.AutoValidateFilter(controllerType))
                {
                    return false;
                }
            }

            if (IsIgnoredHttpMethod(context))
            {
                return false;
            }

            return base.ShouldValidate(context);
        }

        protected virtual bool IsIgnoredHttpMethod(AuthorizationFilterContext context)
        {
            return context.HttpContext
                .Request
                .Method
                .ToUpperInvariant()
                .IsIn(_options.AutoValidateIgnoredHttpMethods);
        }
    }

}
