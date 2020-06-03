using Infotecs.Magazine.Domain.Contracts.Provider;
using Infotecs.Magazine.Infrastracture.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infotecs.Magazine.Infrastracture.DB.Comment
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Статья".
    /// </summary>
    public class CommentService : IEntityService<Domain.Comment.Comment>
    {
        readonly UnitOfWork _unitOfWork;
        readonly IValidateProvider<Domain.Comment.Comment> _commentValidateProvider;

        public CommentService(UnitOfWork unitOfWork,
                              IValidateProvider<Domain.Comment.Comment> commentValidateProvider)
        {
            _unitOfWork = unitOfWork;
            _commentValidateProvider = commentValidateProvider;
        }

        public IQueryable<Domain.Comment.Comment> Get()
        {
            return _unitOfWork.CommentRepository.Include(a => a.Account).AsNoTracking();
        }

        public Domain.Comment.Comment Get(int id)
        {
            return Get().SingleOrDefault(a => a.Id == id);
        }

        public Domain.Comment.Comment Add(Domain.Comment.Comment comment)
        {
            _commentValidateProvider.Validate(comment);

            #region TODO: Add AccountService, Add AccountComponent

            var defaultAccount = _unitOfWork.AccountRepository.FirstOrDefault();

            if (defaultAccount == null)
                throw new ArgumentException("Create at least one account.");

            comment.AccountId = defaultAccount.Id;

            #endregion

            var entry = _unitOfWork.CommentRepository.Add(comment);
            _unitOfWork.Commit();

            return entry.Entity;
        }

        public Domain.Comment.Comment Update(Domain.Comment.Comment comment)
        {
            _commentValidateProvider.Validate(comment);

            var entry = _unitOfWork.CommentRepository.Update(comment);
            _unitOfWork.Commit();

            return entry.Entity;
        }

        public Domain.Comment.Comment Delete(int id)
        {
            var entry = _unitOfWork.CommentRepository.Remove(id);
            _unitOfWork.Commit();

            return Get(entry.Entity.Id);
        }
    }
}
