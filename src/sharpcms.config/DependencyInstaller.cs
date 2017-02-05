using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sharpcms.config
{
    public static class DependencyInstaller
    {
        public static void AddSharpCmsConfig(this IServiceCollection services, IConfigurationBuilder builder)
        {
            services.AddSingleton<IConfigurationService>((provider) => new ConfigurationService(builder));
        }
    }
}