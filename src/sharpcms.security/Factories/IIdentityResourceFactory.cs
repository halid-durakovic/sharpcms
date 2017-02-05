using System.Collections.Generic;
using IdentityServer4.Models;

namespace sharpcms.security.Factories
{
    public interface IIdentityResourceFactory
    {
        List<IdentityResource> GetIdentityResources();
    }
}