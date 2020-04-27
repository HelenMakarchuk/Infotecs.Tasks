using Magazine.Domain.Entities;
using Serilog;
using System;

namespace Infotecs.Magazine.Desktop.Providers
{
    public class AuthenticationProvider
    {
        ILogger _logger;

        public AuthenticationProvider(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Событие завершения аутентификации пользователя.
        /// </summary>
        public event Action LoggedIn;

        /// <summary>
        /// Текущий пользователь приложения.
        /// </summary>
        public Account CurrentAccount { get; private set; }

        /// <summary>
        /// Признак того, что текущий пользователь прошел аутентификацию.
        /// </summary>
        public bool IsLoggedIn => CurrentAccount != null;

        public void LogIn(Account account)
        {
            CurrentAccount = account;
            _logger.Information("New login of {Login}.", CurrentAccount.Login);

            LoggedIn?.Invoke();
        }

        /// <summary>
        /// Выход текущего пользователя из приложения и переход на страницу входа в приложение.
        /// </summary>
        public void LogOut()
        {
            _logger.Information("Logout of {Login}.", CurrentAccount.Login);
            CurrentAccount = null;
        }
    }
}
