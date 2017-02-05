using System.Collections.Generic;
using IdentityServer4.Models;

namespace sharpcms.security.Factories
{
    public interface IApiResourceFactory
    {
        List<ApiResource> GetApiResources();
    }
}