using Magazine.Application.Contracts.Provider;
using Magazine.Application.Contracts.Service;
using Magazine.Domain.Entities;
using NHibernate;
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
        ISessionFactory _sessionFactory;
        ILogger _logger;

        public AuthenticationService(ISessionFactory sessionFactory,
                                     IHashProvider hashProvider,
                                     ILogger logger)
        {
            _sessionFactory = sessionFactory;
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
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                if (session.QueryOver<User>().Where(u => u.Login == login).SingleOrDefault() != null)
                    return false;

                var user = new User();
                user.Login = login;
                user.Salt = _hashProvider.GetSalt();
                user.Password = _hashProvider.GetHash(password, user.Salt);

                session.Save(user);
                transaction.Commit();
            }

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
            using (var session = _sessionFactory.OpenSession())
            {
                var user = session.QueryOver<User>().Where(u => u.Login == login).SingleOrDefault();

                if (user == null)
                    return false;

                var dbHash = user.Password;
                var currentHash = _hashProvider.GetHash(password, user.Salt);

                if (StringComparer.Ordinal.Compare(currentHash, user.Password) != 0)
                {
                    _logger.Warning("Login attempt of {Login}.", login);
                    return false;
                }

                User = user;
                _logger.Information("New login of {Login}.", User.Login);
            }

            return true;
        }

        /// <summary>
        /// Выход текущего пользователя из приложения и переход на страницу входа в приложение.
        /// </summary>
        public void LogOut()
        {
            _logger.Information("Logout of {Login}.", User.Login);
            User = null;
        }
    }
}
