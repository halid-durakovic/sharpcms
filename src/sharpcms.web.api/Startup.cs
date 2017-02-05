using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using sharpcms.content;
using sharpcms.startup;

namespace sharpcms.web.api
{
    public class Startup : AbstractStartup
    {
        public static string Db = "sharpcms";

        public Startup(IHostingEnvironment env) : base(env)
        {
            var contentFragmentService = new ContentFragmentService();

            contentFragmentService.CreateIfDoesNotExist(Db);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddAuthorization();

            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("default");

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                RequireHttpsMetadata = false,
                ApiName = "sharpcms-api",
                ApiSecret = Config.Get("sharpcms-api-client"),
                AllowedScopes = new List<string>() { "sharpcms-api" }
            });

            base.Configure(app, env, loggerFactory);
        }
    }
}
