namespace Magazine.Domain.Contracts.ViewModel
{
    public interface ISignUpViewModel : IViewModel
    {
        bool TrySignUp(string login, string password);
    }
}
