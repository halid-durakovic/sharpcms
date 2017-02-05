using System;

namespace sharpcms.web.id.services.account
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;

        public static bool AllowRememberLogin = true;

        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt = false;

        public static bool AutomaticRedirectAfterSignOut = true;

        public static bool WindowsAuthenticationEnabled = true;

        public static readonly string[] WindowsAuthenticationSchemes = new string[] { "Negotiate", "NTLM" };

        public static readonly string WindowsAuthenticationDisplayName = "Windows";

        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}
