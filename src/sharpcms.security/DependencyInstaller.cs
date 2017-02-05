using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using sharpcms.config;
using sharpcms.security.Factories;

namespace sharpcms.security
{
    public static class DependencyInstaller
    {
        public static void AddSharpCmsSecurity(this IIdentityServerBuilder builder, IConfigurationService config)
        {
            builder.Services.AddTransient<IUserFactory, UserFactory>();

            builder.Services.AddTransient<IIdentityResourceFactory, IdentityResourceFactory>();

            builder.Services.AddTransient<IApiResourceFactory, ApiResourceFactory>();

            builder.Services.AddTransient<IClientFactory, ClientFactory>();

            builder.Services.AddTransient<IUserStore, UserStore>();

            builder.Services.AddTransient<IResourceOwnerPasswordValidator, UserResourceOwnerPasswordValidator>();

            builder.Services.AddTransient<IProfileService, UserProfileService>();

            var identityResourceFactory = new IdentityResourceFactory();

            builder.AddInMemoryIdentityResources(identityResourceFactory.GetIdentityResources());

            var apiResourceFactory = new ApiResourceFactory();

            builder.AddInMemoryApiResources(apiResourceFactory.GetApiResources());

            var clientFactory = new ClientFactory();

            builder.AddInMemoryClients(clientFactory.GetClients(config));
        }
    }
}