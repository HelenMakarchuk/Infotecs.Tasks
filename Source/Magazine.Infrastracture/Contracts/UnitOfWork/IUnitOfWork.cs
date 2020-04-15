using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.Repository;

namespace Magazine.Infrastracture.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        void Commit();
    }
}
