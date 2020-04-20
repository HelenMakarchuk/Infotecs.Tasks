using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.ViewModel;
using NHibernate;
using Serilog;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы входа в приложение <see cref="LogInPage"/>.
    /// </summary>
    public class LogInViewModel : ILogInViewModel
    {
        IAuthenticationService _authenticationService;
        ISessionFactory _sessionFactory;
        ILogger _logger;

        public LogInViewModel(ISessionFactory sessionFactory,
                              IAuthenticationService authenticationService,
                              ILogger logger)
        {
            _sessionFactory = sessionFactory;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Возвращается True если аутентификация выполнена, иначе False.</returns>
        public bool TryLogIn(string login, string password)
        {
            var loginAttemptResult = _authenticationService.TryLogIn(login, password);

            _logger.Debug("Log in attempt result: {Result}.", loginAttemptResult);

            return loginAttemptResult;
        }
    }
}
