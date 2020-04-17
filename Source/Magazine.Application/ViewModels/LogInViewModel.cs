using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Serilog;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы входа в приложение <see cref="LogInPage"/>.
    /// </summary>
    public class LogInViewModel : ILogInViewModel
    {
        IAuthenticationService _authenticationService;
        IUnitOfWork _unitOfWork;
        ILogger _logger;

        public LogInViewModel(IUnitOfWork unitOfWork,
                              IAuthenticationService authenticationService,
                              ILogger logger)
        {
            _unitOfWork = unitOfWork;
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
            return _authenticationService.TryLogIn(login, password);
        }
    }
}
