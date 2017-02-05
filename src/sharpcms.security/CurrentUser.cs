using System.Linq;
using System.Security.Claims;

namespace sharpcms.security
{
    public class CurrentUser
    {
        private readonly ClaimsPrincipal _principal;

        public static CurrentUser For(ClaimsPrincipal principal)
        {
            return new CurrentUser(principal);
        }

        public CurrentUser(ClaimsPrincipal principal)
        {
            _principal = principal;
        }

        public bool IsValid
        {
            get
            {
                if (_principal == null)
                    return false;

                if (!_principal.Claims.Any())
                    return false;

                return true;
            }
        }

        public string Name
        {
            get
            {
                if (_principal == null)
                    return "Unknown";

                if (!_principal.Claims.Any())
                    return "Unknown";

                return _principal.Claims.FirstOrDefault(x => x.Type == "name").Value;
            }
        }
    }
}