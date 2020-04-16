using Magazine.Application.Contracts.Provider;
using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Infrastracture.Contracts.UnitOfWork;

namespace Magazine.Application.ViewModels
{
    class SignUpViewModel : ISignUpViewModel
    {
        IHashProvider _passwordProvider;
        IUnitOfWork _unitOfWork;
        IAuthenticationService _authenticationService;

        public SignUpViewModel(IAuthenticationService authenticationService,
                               IUnitOfWork unitOfWork)
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
        }

        public bool TrySignUp(string login, string password)
        {
            return _authenticationService.TrySignUp(login, password);
        }
    }
}
