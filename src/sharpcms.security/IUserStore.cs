using System.Collections.Generic;
using System.Security.Claims;
using sharpcms.security.model;

namespace sharpcms.security
{
    public interface IUserStore
    {
        bool ValidateCredentials(string username, string password);

        UserModel FindBySubjectId(string subjectId);

        UserModel FindByUsername(string username);

        UserModel FindByExternalProvider(string provider, string userId);

        UserModel AutoProvisionUser(string provider, string userId, List<Claim> claims);
    }
}