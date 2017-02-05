using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace sharpcms.security
{
    public class UserProfileService : IProfileService
    {
        private readonly IUserStore _users;

        public UserProfileService(IUserStore users)
        {
            _users = users;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var bySubjectId = _users.FindBySubjectId(context.Subject.GetSubjectId());
            context.AddFilteredClaims(bySubjectId.Claims);
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}