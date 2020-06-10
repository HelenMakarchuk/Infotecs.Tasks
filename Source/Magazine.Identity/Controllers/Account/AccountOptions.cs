using System;

namespace Infotecs.Identity
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;
        public static string LoginExistsErrorMessage = "User with this login already exists";
        public static string IncorrectCredentialsErrorMessage = "Incorrect username or password";
    }
}
