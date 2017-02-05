using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using sharpcms.config;
using sharpcms.content;
using sharpcms.database;
using sharpcms.json;

namespace sharpcms.startup
{
    public abstract class AbstractStartup
    {
        private readonly IConfigurationBuilder _builder;

        protected readonly IConfigurationService Config;

        protected AbstractStartup(IHostingEnvironment env)
        {
            _builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddJsonFile("settings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets();

            if (env.IsDevelopment())
            {
            }

            Config = new ConfigurationService(_builder);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSharpCmsConfig(_builder);

            services.AddSharpCmsDatabase();

            services.AddSharpCmsContent();

            services.AddSharpCmsJson();

            services.AddMvc();
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
