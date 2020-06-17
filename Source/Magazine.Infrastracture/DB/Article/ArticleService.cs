using Infotecs.Magazine.Domain.Contracts.Provider;
using Infotecs.Magazine.Infrastracture.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infotecs.Magazine.Infrastracture.DB.Article
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Статья".
    /// </summary>
    public class ArticleService : IEntityService<Domain.Article.Article>
    {
        readonly UnitOfWork _unitOfWork;
        readonly IValidateProvider<Domain.Article.Article> _articleValidateProvider;

        public ArticleService(UnitOfWork unitOfWork,
                              IValidateProvider<Domain.Article.Article> articleValidateProvider)
        {
            _unitOfWork = unitOfWork;
            _articleValidateProvider = articleValidateProvider;
        }

        public IQueryable<Domain.Article.Article> Get()
        {
            return _unitOfWork.ArticleRepository.Include(a => a.Account).AsNoTracking();
        }

        public Domain.Article.Article Get(int id)
        {
            return Get().SingleOrDefault(a => a.Id == id);
        }

        public Domain.Article.Article Add(Domain.Article.Article article)
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

        public Domain.Article.Article Update(Domain.Article.Article article)
        {
            _articleValidateProvider.Validate(article);

            var entry = _unitOfWork.ArticleRepository.Update(article);
            _unitOfWork.Commit();

            return entry.Entity;
        }

        public Domain.Article.Article Delete(int id)
        {
            var commentIds = _unitOfWork.CommentRepository.Where(c => c.ArticleId == id).Select(c => c.Id).ToArray();

            foreach (var commentId in commentIds)
                _unitOfWork.CommentRepository.Remove(commentId);

            var entry = _unitOfWork.ArticleRepository.Remove(id);
            _unitOfWork.Commit();

            return entry.Entity;
        }
    }
}
