using Magazine.Domain.Contracts.Entity;
using Magazine.Infrastracture.Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Magazine.Infrastracture.DB.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        DbContext _context;
        protected DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public EntityEntry<T> Add(T entity)
        {
            return _entities.Add(entity);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _entities.Any(predicate);
        }

        public T Find(params object[] keyValues)
        {
            return _entities.Find(keyValues);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _entities.FirstOrDefault(predicate);
        }

        public EntityEntry<T> Remove(int id)
        {
            return _entities.Remove(Find(id));
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entities.AsNoTracking();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
