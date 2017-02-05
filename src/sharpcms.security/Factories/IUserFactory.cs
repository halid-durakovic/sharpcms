using System.Collections.Generic;
using sharpcms.config;
using sharpcms.security.model;

namespace sharpcms.security.Factories
{
    public interface IUserFactory
    {
        List<UserModel> GetUsers(IConfigurationService config);
    }
}