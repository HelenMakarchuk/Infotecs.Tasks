using Magazine.Application.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;

namespace Magazine.Application.ViewModels
{
    class NewArticleViewModel : INewArticleViewModel
    {
        IUnitOfWork _unitOfWork;

        public NewArticleViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Save(string title, string body, int userId, byte[] teaser = null)
        {
            var article = new Article(title, body, userId, teaser);

            _unitOfWork.ArticleRepository.Add(article);
            _unitOfWork.Commit();
        }
    }
}
