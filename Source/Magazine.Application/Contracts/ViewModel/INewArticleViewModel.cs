using Magazine.Domain.Contracts.ViewModel;

namespace Magazine.Application.Contracts.ViewModel
{
    public interface INewArticleViewModel : IViewModel
    {
        void Save(string title, string body, int userId, byte[] teaser = null);
    }
}
