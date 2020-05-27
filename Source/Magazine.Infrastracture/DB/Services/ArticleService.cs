using Infotecs.Magazine.Domain.Contracts.Provider;
using Infotecs.Magazine.Domain.Entities;
using Infotecs.Magazine.Infrastracture.Contracts.Service;
using System;
using System.Linq;

namespace Infotecs.Magazine.Infrastracture.DB.Services
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Статья".
    /// </summary>
    public class ArticleService : IEntityService<Article>
    {
        readonly UnitOfWork _unitOfWork;
        readonly IValidateProvider<Article> _articleValidateProvider;

        public ArticleService(UnitOfWork unitOfWork,
                              IValidateProvider<Article> articleValidateProvider)
        {
            _unitOfWork = unitOfWork;
            _articleValidateProvider = articleValidateProvider;
        }

        public IQueryable<Article> Get()
        {
            return _unitOfWork.ArticleRepository.AsNoTracking();
        }

        public Article Get(int id)
        {
            return Get().SingleOrDefault(a => a.Id == id);
        }

        public Article Add(Article article)
        {
            _articleValidateProvider.Validate(article);

            #region TODO: Add AccountService, Add AccountComponent

            var defaultAccount = _unitOfWork.AccountRepository.FirstOrDefault();

            if (defaultAccount == null)
                throw new ArgumentException("Create at least one account.");

            article.AccountId = defaultAccount.Id;

            #endregion

            var dbArticle = _unitOfWork.ArticleRepository.SingleOrDefault(a => a.Title == article.Title);

            if (dbArticle != null)
                throw new ArgumentException("This title already exists.");

            var entry = _unitOfWork.ArticleRepository.Add(article);
            _unitOfWork.Commit();

            return entry.Entity;
        }

        public Article Update(Article article)
        {
            _articleValidateProvider.Validate(article);

            var entry = _unitOfWork.ArticleRepository.Update(article);
            _unitOfWork.Commit();

            return entry.Entity;
        }

        public Article Delete(int id)
        {
            var entry = _unitOfWork.ArticleRepository.Remove(id);
            _unitOfWork.Commit();

            return Get(entry.Entity.Id);
        }
    }
}
