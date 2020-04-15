using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;

namespace Magazine.Application.ViewModels
{
    class LogInViewModel : ILogInViewModel
    {
        public User CurrentUser { get; set; }
    }
}
