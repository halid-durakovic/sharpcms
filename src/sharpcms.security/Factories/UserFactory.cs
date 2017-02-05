using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using sharpcms.config;
using sharpcms.security.model;

namespace sharpcms.security.Factories
{
    public class UserFactory : IUserFactory
    {
        public List<UserModel> GetUsers(IConfigurationService config)
        {
            var results = new List<UserModel>();

            var alice = new UserModel{SubjectId = "1", Username = "admin", Password = config.Get("admin"),
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Administrator"),
                    new Claim(JwtClaimTypes.GivenName, ""),
                    new Claim(JwtClaimTypes.FamilyName, ""),
                    new Claim(JwtClaimTypes.Email, "admin@sharpcms"),
                    new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://admin"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("role", "admin")
                }
            };

            results.Add(alice);

            var bob = new UserModel{SubjectId = "2", Username = "user", Password = config.Get("user"),
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "User"),
                    new Claim(JwtClaimTypes.GivenName, ""),
                    new Claim(JwtClaimTypes.FamilyName, ""),
                    new Claim(JwtClaimTypes.Email, "user@sharpcms"),
                    new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://user"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("role", "user")
                }
            };

            results.Add(bob);

            return results;
        }
    }
}