namespace Magazine.Domain.Contracts.ViewModel
{
    public interface ILogInViewModel : IViewModel
    {
        bool TryLogIn(string login, string password);
    }
}
