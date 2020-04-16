using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Infrastracture.Contracts.UnitOfWork;

namespace Magazine.Application.ViewModels
{
    class LogInViewModel : ILogInViewModel
    {
        IAuthenticationService _authenticationService;
        IUnitOfWork _unitOfWork;

        public LogInViewModel(IUnitOfWork unitOfWork,
                              IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

        public bool TryLogIn(string login, string password)
        {
            return _authenticationService.TryLogIn(login, password);
        }
    }
}
