using Magazine.Domain.Entities;

namespace Magazine.Application.Contracts.Service
{
    /// <summary>
    /// Интерфейс сервиса аутентификации.
    /// </summary>
    public interface IAuthenticationService
    {
        public Account User { get; }
        bool IsLoggedIn { get; }
        bool TrySignUp(string username, string password);
        bool TryLogIn(string login, string password);
        void LogOut();
    }
}
