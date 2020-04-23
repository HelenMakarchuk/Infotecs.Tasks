using Magazine.Domain.Entities;
using System;

namespace Infotecs.Magazine.Application.Contracts.ViewModel
{
    public interface IApplicationViewModel
    {
        event Action LoggedIn;

        Account CurrentAccount { get; }
        bool IsLoggedIn { get; }

        void LogIn(Account account);
        void LogOut();
    }
}
