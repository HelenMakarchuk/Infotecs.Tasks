using Magazine.Domain.Entities;

namespace Magazine.Domain.Contracts.ViewModel
{
    public interface ILogInViewModel : IViewModel
    {
        User CurrentUser { get; set; }
    }
}
