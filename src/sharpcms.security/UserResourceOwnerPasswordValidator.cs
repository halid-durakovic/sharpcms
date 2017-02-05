using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace sharpcms.security
{
    public class UserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserStore _users;

        public UserResourceOwnerPasswordValidator(IUserStore users)
        {
            _users = users;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_users.ValidateCredentials(context.UserName, context.Password))
            {
                var byUsername = _users.FindByUsername(context.UserName);
                context.Result = new GrantValidationResult(byUsername.SubjectId, "pwd", byUsername.Claims, "local", null);
            }
            return Task.FromResult(0);
        }
    }
}