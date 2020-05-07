using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;

namespace Infotecs.Magazine.Infrastracture.DB.Services
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Статья".
    /// </summary>
    public class CommentService : IEntityService<Comment>
    {
        UnitOfWork _unitOfWork;

        public CommentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Comment> Get()
        {
            return _unitOfWork.CommentRepository.AsNoTracking().Include(c => c.Account).Include(c => c.Article);
        }

        public Comment Get(int id)
        {
            return Get().SingleOrDefault(c => c.Id == id);
        }

        public Comment Update(Comment comment)
        {
            var entry = _unitOfWork.CommentRepository.Update(comment);
            _unitOfWork.Commit();

            entry.State = EntityState.Detached;

            return Get(entry.Entity.Id);
        }

        public Comment Delete(int id)
        {
            var entry = _unitOfWork.CommentRepository.Remove(id);
            _unitOfWork.Commit();

            entry.State = EntityState.Detached;

            return Get(entry.Entity.Id);
        }

        public Comment Add(Comment comment)
        {
            var entry = _unitOfWork.CommentRepository.Add(comment);
            _unitOfWork.Commit();

            return comment;
        }
    }
}
