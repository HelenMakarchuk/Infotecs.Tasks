namespace Magazine.Domain.Contracts.ViewModel
{
    /// <summary>
    /// Интерфейс класса бизнес-логики <see cref="SignUpViewModel"/> для страницы создания нового пользователя приложения <see cref="SignUpPage"/>.
    /// </summary>
    public interface ISignUpViewModel
    {
        void SignUp(string login, string password);
    }
}
