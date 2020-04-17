namespace Magazine.Domain.Contracts.ViewModel
{
    /// <summary>
    /// Интерфейс класса бизнес-логики <see cref="SignUpViewModel"/> для страницы создания нового пользователя приложения <see cref="SignUpPage"/>.
    /// </summary>
    public interface ISignUpViewModel
    {
        bool TrySignUp(string login, string password);
    }
}
