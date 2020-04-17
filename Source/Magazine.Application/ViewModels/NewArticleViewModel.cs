using Magazine.Application.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Serilog;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы создания новой статьи <see cref="NewArticlePage"/>.
    /// </summary>
    public class NewArticleViewModel : INewArticleViewModel
    {
        IUnitOfWork _unitOfWork;
        ILogger _logger;

        public NewArticleViewModel(IUnitOfWork unitOfWork,
                                   ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Создание новой статьи.
        /// </summary>
        /// <param name="title">Заголовок статьи.</param>
        /// <param name="body">Контент статьи.</param>
        /// <param name="userId">Идентификатор автора статьи.</param>
        /// <param name="teaser">Картинка-тизер.</param>
        public void Save(string title, string body, int userId, byte[] teaser = null)
        {
            var article = new Article(title, body, userId, teaser);

            _unitOfWork.ArticleRepository.Add(article);
            _unitOfWork.Commit();
        }
    }
}
