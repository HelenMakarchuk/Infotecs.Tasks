using Magazine.Domain.Contracts.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
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
        IQueryable<TReturn> Select<TReturn>(Expression<Func<T, TReturn>> selector);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        EntityEntry<T> Add(T entity);
        EntityEntry<T> Remove(int id);
        IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
        EntityEntry<T> Update(T entity);
    }
}