using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Serilog;

namespace Magazine.Application.ViewModels
{
    public class SignUpViewModel : ISignUpViewModel
    {
        IUnitOfWork _unitOfWork;
        IAuthenticationService _authenticationService;
        ILogger _logger;

        public SignUpViewModel(IAuthenticationService authenticationService,
                               IUnitOfWork unitOfWork,
                               ILogger logger)
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public bool TrySignUp(string login, string password)
        {
            return _authenticationService.TrySignUp(login, password);
        }
    }
}
