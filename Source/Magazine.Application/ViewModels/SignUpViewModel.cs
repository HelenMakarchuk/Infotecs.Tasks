using Magazine.Application.Contracts.Provider;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;

namespace Magazine.Application.ViewModels
{
    class SignUpViewModel : ISignUpViewModel
    {
        IPasswordProvider _passwordProvider;
        IUnitOfWork _unitOfWork;

        public SignUpViewModel(IPasswordProvider passwordProvider,
                                IUnitOfWork unitOfWork)
        {
            _passwordProvider = passwordProvider;
            _unitOfWork = unitOfWork;
        }

        public void SignUp(string login, string password)
        {
            var user = new User();
            user.Login = login;
            user.Salt = _passwordProvider.GetSalt();
            user.Password = _passwordProvider.GetHash(password, user.Salt);

            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Commit();
        }
    }
}
