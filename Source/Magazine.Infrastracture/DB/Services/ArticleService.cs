using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infotecs.Magazine.Infrastracture.DB.Services
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Статья".
    /// </summary>
    public class ArticleService : IEntityService<Article>
    {
        UnitOfWork _unitOfWork;
        ILogger _logger;

        public ArticleService(UnitOfWork unitOfWork,
                              ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public List<Article> Get()
        {
            return _unitOfWork.ArticleRepository.AsNoTracking().Select(a => new Article() { Id = a.Id, Title = a.Title }).ToList();
        }

        public Article Get(int id)
        {
            return _unitOfWork.ArticleRepository.AsNoTracking().Include(a => a.Comments).ThenInclude(c => c.Account).SingleOrDefault(a => a.Id == id);
        }

        public Article Add(Article article)
        {
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

            entry.State = EntityState.Detached;

            return Get(entry.Entity.Id);
        }

        public Article Update(Article article)
        {
            var entry = _unitOfWork.ArticleRepository.Update(article);
            _unitOfWork.Commit();

            entry.State = EntityState.Detached;

            return Get(entry.Entity.Id);
        }

        public Article Delete(int id)
        {
            var entry = _unitOfWork.ArticleRepository.Remove(id);
            _unitOfWork.Commit();

            entry.State = EntityState.Detached;

            return Get(entry.Entity.Id);
        }
    }
}
