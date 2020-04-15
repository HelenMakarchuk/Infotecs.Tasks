using Magazine.Domain.Contracts.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Magazine.Infrastracture.Contracts.Repository
{
    public interface IRepository<T> where T : class, IEntity
    {
        bool Any(Expression<Func<T, bool>> predicate);
        T Find(params object[] keyValues);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        EntityEntry<T> Add(T entity);
        EntityEntry<T> Remove(int id);
        IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties);
    }
}