using Magazine.Domain.Entities;

namespace Magazine.Application.Contracts.Service
{
    public interface IAuthenticationService
    {
        public User User { get; }
        bool IsLoggedIn { get; }
        bool TrySignUp(string username, string password);
        bool TryLogIn(string login, string password);
        void LogOut();
    }
}
