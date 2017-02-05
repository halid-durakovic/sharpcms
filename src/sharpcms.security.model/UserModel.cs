using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;

namespace sharpcms.security.model
{
    public class UserModel
    {
        public ICollection<Claim> Claims { get; set; } = (ICollection<Claim>) new HashSet<Claim>(new ClaimComparer());

        public string SubjectId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ProviderName { get; set; }

        public string ProviderSubjectId { get; set; }
    }
}