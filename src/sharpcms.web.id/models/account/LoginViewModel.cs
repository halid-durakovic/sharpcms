using System.Collections.Generic;
using System.Linq;

namespace sharpcms.web.id.models.account
{
    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; }

        public bool EnableLocalLogin { get; set; }

        public IEnumerable<ExternalProviderViewModel> ExternalProviders { get; set; }

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
    }
}