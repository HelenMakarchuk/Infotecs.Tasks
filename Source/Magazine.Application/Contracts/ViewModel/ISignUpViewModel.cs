namespace Magazine.Domain.Contracts.ViewModel
{
    public interface ISignUpViewModel : IViewModel
    {
        void SignUp(string login, string password);
    }
}
