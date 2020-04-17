using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Serilog;

namespace Magazine.Application.ViewModels
{
    public class LogInViewModel : ILogInViewModel
    {
        IAuthenticationService _authenticationService;
        IUnitOfWork _unitOfWork;
        ILogger _logger;

        public LogInViewModel(IUnitOfWork unitOfWork,
                              IAuthenticationService authenticationService,
                              ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public bool TryLogIn(string login, string password)
        {
            return _authenticationService.TryLogIn(login, password);
        }
    }
}
