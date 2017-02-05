using Microsoft.Extensions.DependencyInjection;

namespace sharpcms.json
{
    public static class DependencyInstaller
    {
        public static void AddSharpCmsJson(this IServiceCollection services)
        {
            services.AddTransient<IJsonService, JsonService>();
        }
    }
}