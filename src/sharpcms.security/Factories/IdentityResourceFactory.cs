using System.Collections.Generic;
using IdentityServer4.Models;

namespace sharpcms.security.Factories
{
    public class IdentityResourceFactory : IIdentityResourceFactory
    {
        public List<IdentityResource> GetIdentityResources()
        {
            var openIdResource = new IdentityResources.OpenId();

            openIdResource.UserClaims.Add("role");

            var profileResource = new IdentityResources.Profile();

            profileResource.UserClaims.Add("role");

            return new List<IdentityResource>
            {
                openIdResource,
                profileResource
            };
        }
    }
}