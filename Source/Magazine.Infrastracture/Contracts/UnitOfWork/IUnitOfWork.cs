using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.Repository;

namespace Magazine.Infrastracture.Contracts.UnitOfWork
{
    /// <summary>
    /// Интерфейс класса, реализующего паттерн UnitOfWork <see cref="UnitOfWork"/>.
    /// </summary>
    public interface IUnitOfWork
    {
        IRepository<Account> UserRepository { get; }
        IRepository<Article> ArticleRepository { get; }
        public IRepository<Comment> CommentRepository { get; }
        void Commit();
    }
}
