using System.Collections.Generic;
using IdentityServer4.Models;

namespace sharpcms.security.Factories
{
    public class ApiResourceFactory : IApiResourceFactory
    {
        public List<ApiResource> GetApiResources()
        {
            var apiResource = new ApiResource("sharpcms-api", "SharpCMS API");

            return new List<ApiResource>
            {
                apiResource
            };
        }
    }
}