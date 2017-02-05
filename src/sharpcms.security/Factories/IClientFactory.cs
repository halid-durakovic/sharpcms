using System.Collections.Generic;
using IdentityServer4.Models;
using sharpcms.config;

namespace sharpcms.security.Factories
{
    public interface IClientFactory
    {
        List<Client> GetClients(IConfigurationService config);
    }
}