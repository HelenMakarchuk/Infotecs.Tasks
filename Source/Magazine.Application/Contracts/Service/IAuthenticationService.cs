using Magazine.Domain.Entities;

namespace Magazine.Application.Contracts.Service
{
    interface IAuthenticationService
    {
        bool IsLoggedIn { get; }
        bool TrySignUp(string username, string password);
        bool TryLogIn(string login, string password);
        void LogOut();
    }
}
