using System;

namespace Magazine.Domain.Contracts.ViewModel
{
    /// <summary>
    /// Интерфейс класса бизнес-логики <see cref="LogInViewModel"/> для страницы входа в приложение <see cref="LogInPage"/>.
    /// </summary>
    public interface ILogInViewModel
    {
        event Action LoggedIn;

        void LogIn(string login, string password);
    }
}
