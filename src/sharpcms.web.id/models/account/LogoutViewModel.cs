namespace sharpcms.web.id.models.account
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; }

        public string PostLogoutRedirectUrl { get; set; }
    }
}
