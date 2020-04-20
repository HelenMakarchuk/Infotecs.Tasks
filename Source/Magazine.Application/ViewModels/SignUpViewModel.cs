using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Serilog;
using System.ComponentModel;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы создания нового пользователя приложения <see cref="SignUpPage"/>.
    /// </summary>
    public class SignUpViewModel : ISignUpViewModel
    {
        IUnitOfWork _unitOfWork;
        IAuthenticationService _authenticationService;
        ILogger _logger;

        public SignUpViewModel(IAuthenticationService authenticationService,
                               IUnitOfWork unitOfWork,
                               ILogger logger)
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Регистрация нового пользователя приложения.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Возвращается True если регистрация выполнена, иначе False.</returns>
        public bool TrySignUp(string login, string password)
        {
            var signUpAttemptResult = _authenticationService.TrySignUp(login, password);

            _logger.Debug("Sign up attempt result: {Result}.", signUpAttemptResult);

            return signUpAttemptResult;
        }
    }
}
