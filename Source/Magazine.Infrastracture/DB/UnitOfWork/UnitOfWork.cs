using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.Repository;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;

namespace Magazine.Infrastracture.DB.UnitOfWork
{
    /// <summary>
    /// Класс реализует паттерн UnitOfWork.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        DbContext _context;
        bool _disposed;

        public UnitOfWork(DbContext context,
                          IRepository<User> userRepository,
                          IRepository<Article> articleRepository,
                          IRepository<Comment> commentRepository)
        {
            _context = context;
            UserRepository = userRepository;
            ArticleRepository = articleRepository;
            CommentRepository = commentRepository;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        public IRepository<User> UserRepository { get; }
        public IRepository<Article> ArticleRepository { get; }
        public IRepository<Comment> CommentRepository { get; }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _context?.Database?.EnsureDeleted();
                _context?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}