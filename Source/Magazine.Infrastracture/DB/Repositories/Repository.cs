using Magazine.Domain.Contracts.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Magazine.Infrastracture.DB.Repositories
{
    /// <summary>
    /// Обобщенный репозиторий для работы с сущностями БД.
    /// </summary>
    /// <typeparam name="T">Тип сущности БД.</typeparam>
    public class Repository<T> where T : class, IEntity
    {
        protected DbContext _context;
        protected DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public IQueryable<T> AsNoTracking()
        {
            return _entities.AsNoTracking();
        }

        public EntityEntry<T> Add(T entity)
        {
            return _entities.Add(entity);
        }

        public IQueryable<TReturn> Select<TReturn>(Expression<Func<T, TReturn>> selector)
        {
            return _entities.Select(selector);
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

        public T FirstOrDefault()
        {
            return _entities.FirstOrDefault();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public EntityEntry<T> Remove(int id)
        {
            return _entities.Remove(Find(id));
        }

        public IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return _entities.Include(navigationPropertyPath);
        }

        public EntityEntry<T> Update(T entity)
        {
            return _entities.Update(entity);
        }
    }
}
