using Magazine.Application.Contracts.Provider;
using Magazine.Application.Contracts.Service;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Serilog;
using System;

namespace Magazine.Application.Services
{
    /// <summary>
    /// Сервис аутентификации пользователя приложения.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        IHashProvider _hashProvider;
        IUnitOfWork _unitOfWork;
        ILogger _logger;

        public AuthenticationService(IUnitOfWork unitOfWork,
                                     IHashProvider hashProvider,
                                     ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _hashProvider = hashProvider;
            _logger = logger;
        }

        /// <summary>
        /// Текущий пользователь приложения.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Признак того, что текущий пользователь прошел аутентификацию.
        /// </summary>
        public bool IsLoggedIn => User != null;

        /// <summary>
        /// Регистрация нового пользователя приложения.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Возвращается True если регистрация выполнена, иначе False.</returns>
        public bool TrySignUp(string login, string password)
        {
            if (_unitOfWork.UserRepository.SingleOrDefault(u => u.Login == login) != null)
                return false;

            var user = new User();
            user.Login = login;
            user.Salt = _hashProvider.GetSalt();
            user.Password = _hashProvider.GetHash(password, user.Salt);

            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Commit();

            return TryLogIn(login, password);
        }

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Возвращается True если аутентификация выполнена, иначе False.</returns>
        public bool TryLogIn(string login, string password)
        {
            var user = _unitOfWork.UserRepository.SingleOrDefault(u => u.Login == login);

            if (user == null)
                return false;

            var dbHash = user.Password;
            var currentHash = _hashProvider.GetHash(password, user.Salt);

            if (StringComparer.Ordinal.Compare(currentHash, user.Password) != 0)
                return false;

            User = user;
            return true;
        }

        /// <summary>
        /// Выход текущего пользователя из приложения и переход на страницу входа в приложение.
        /// </summary>
        public void LogOut()
        {
            User = null;
        }
    }
}
