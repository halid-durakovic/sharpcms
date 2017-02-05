using Microsoft.Extensions.DependencyInjection;

namespace sharpcms.content
{
    public static class DependencyInstaller
    {
        public static void AddSharpCmsContent(this IServiceCollection services)
        {
            services.AddTransient<IContentFragmentService, ContentFragmentService>();

            services.AddTransient<IContentFragmentQueryService, ContentFragmentQueryService>();
        }
    }
}