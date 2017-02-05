using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using sharpcms.config;
using sharpcms.security.Factories;
using sharpcms.security.model;

namespace sharpcms.security
{
    public class UserStore : IUserStore
    {
        private readonly List<UserModel> _users;

        public UserStore(IUserFactory userFactory, IConfigurationService config)
        {
            this._users = userFactory.GetUsers(config);
        }

        public bool ValidateCredentials(string username, string password)
        {
            var byUsername = FindByUsername(username);
            if (byUsername != null)
                return byUsername.Password.Equals(password);
            return false;
        }

        public UserModel FindBySubjectId(string subjectId)
        {
            return _users.FirstOrDefault(x => x.SubjectId == subjectId);
        }

        public UserModel FindByUsername(string username)
        {
            return _users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public UserModel FindByExternalProvider(string provider, string userId)
        {
            return _users.FirstOrDefault(x =>
            {
                if (x.ProviderName == provider)
                    return x.ProviderSubjectId == userId;
                return false;
            });
        }

        public UserModel AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            var allowedClaims = new List<Claim>();

            foreach (var current in claims)
            {
                if (current.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                    allowedClaims.Add(new Claim("name", current.Value));
                else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(current.Type))
                    allowedClaims.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[current.Type], current.Type));
                else
                    allowedClaims.Add(current);
            }

            var uniqueId = CryptoRandom.CreateUniqueId(32);
            var claim = allowedClaims.FirstOrDefault(c => c.Type == "name");
            var str = (claim != null ? claim.Value : null) ?? uniqueId;
            var user = new UserModel
            {
                SubjectId = uniqueId,
                Username = str,
                ProviderName = provider,
                ProviderSubjectId = userId,
                Claims = allowedClaims
            };

            _users.Add(user);

            return user;
        }
    }
}