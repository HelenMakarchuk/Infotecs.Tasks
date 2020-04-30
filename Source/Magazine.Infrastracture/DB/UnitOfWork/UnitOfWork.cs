using Magazine.Domain.Entities;
using Magazine.Infrastracture.DB.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace Magazine.Infrastracture.DB.UnitOfWork
{
    /// <summary>
    /// Класс реализует паттерн UnitOfWork.
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        DbContext _context;
        bool _disposed;

        public UnitOfWork(DbContext context,
                          Repository<Account> userRepository,
                          Repository<Article> articleRepository,
                          Repository<Comment> commentRepository)
        {
            _context = context;
            AccountRepository = userRepository;
            ArticleRepository = articleRepository;
            CommentRepository = commentRepository;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        public Repository<Account> AccountRepository { get; }
        public Repository<Article> ArticleRepository { get; }
        public Repository<Comment> CommentRepository { get; }

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