using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using sharpcms.config;

namespace sharpcms.security.Factories
{
    public class ClientFactory : IClientFactory
    {
        public List<Client> GetClients(IConfigurationService configurationService)
        {
            var apiClient = "sharpcms-api-client";

            var apiClientSecretCfg = configurationService.Get("sharpcms-api-client");

            Console.WriteLine($"SharpCms iD Client: {apiClient}:{apiClientSecretCfg}");

            var apiClientSecret = new Secret(apiClientSecretCfg.Sha256());

            var apiClientDefinition = CreateApiClientDefinition(apiClient, apiClientSecret);

            var mvcClient = "sharpcms-mvc-client";

            var mvcClientSecretCfg = configurationService.Get("sharpcms-mvc-client");

            Console.WriteLine($"SharpCms iD Client: {mvcClient}:{mvcClientSecretCfg}");

            var mvcClientSecret = new Secret(mvcClientSecretCfg.Sha256());

            var mvcClientDefinition = CreateMvcClientDefinition(mvcClient, mvcClientSecret);

            return new List<Client>
            {
                apiClientDefinition,
                mvcClientDefinition
            };
        }

        private static Client CreateMvcClientDefinition(string mvcClient, Secret mvcClientSecret)
        {
            return new Client
            {
                ClientId = mvcClient,
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                ClientSecrets = { mvcClientSecret },
                RedirectUris = {"http://localhost:5002/signin-oidc"},
                PostLogoutRedirectUris = {"http://localhost:5002"},
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "role", "sharpcms-api"
                },
                AllowOfflineAccess = true,
                RequireConsent = false,
                AlwaysIncludeUserClaimsInIdToken = true
            };
        }

        private static Client CreateApiClientDefinition(string apiClient, Secret apiClientSecret)
        {
            return new Client
            {
                ClientId = apiClient,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { apiClientSecret },
                AllowedScopes = { "sharpcms-api" }
            };
        }
    }
}