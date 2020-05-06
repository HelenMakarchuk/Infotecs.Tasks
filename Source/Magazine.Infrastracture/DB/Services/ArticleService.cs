using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Magazine.Domain.Entities;
using Magazine.Domain.Providers;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;

namespace Infotecs.Magazine.Infrastracture.DB.Services
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Статья".
    /// </summary>
    public class ArticleService : IEntityService<Article>
    {
        UnitOfWork _unitOfWork;
        ArticleValidateProvider _articleValidateProvider;
        ILogger _logger;

        public ArticleService(UnitOfWork unitOfWork,
                              ArticleValidateProvider articleValidateProvider,
                              ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _articleValidateProvider = articleValidateProvider;
            _logger = logger;
        }

        public IQueryable<Article> Get()
        {
            return _unitOfWork.ArticleRepository.AsNoTracking().Include(a => a.Account).Include(a => a.Comments).ThenInclude(c => c.Account);
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

            return entry.Entity; //Get(entry.Entity.Id);
        }

        public Article Delete(int id)
        {
            var entry = _unitOfWork.ArticleRepository.Remove(id);
            _unitOfWork.Commit();

            return Get(entry.Entity.Id);
        }
    }
}
