using Microsoft.Extensions.DependencyInjection;
using sharpcms.database.config;

namespace sharpcms.database
{
    public static class DependencyInstaller
    {
        public static void AddSharpCmsDatabase(this IServiceCollection services)
        {
            services.AddTransient<IDbConnectionService, DbConnectionService>();

            services.AddTransient<IDbConnectionConfigService, DbConnectionConfigService>();
        }
    }
}