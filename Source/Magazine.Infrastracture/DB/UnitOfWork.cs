using Microsoft.EntityFrameworkCore;
using System;

namespace Infotecs.Magazine.Infrastracture.DB
{
    /// <summary>
    /// Класс реализует паттерн UnitOfWork.
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        DbContext _context;
        bool _disposed;

        public UnitOfWork(Context context,
                          Repository<Domain.Account.Account> userRepository,
                          Repository<Domain.Article.Article> articleRepository,
                          Repository<Domain.Comment.Comment> commentRepository)
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

        public Repository<Domain.Account.Account> AccountRepository { get; }
        public Repository<Domain.Article.Article> ArticleRepository { get; }
        public Repository<Domain.Comment.Comment> CommentRepository { get; }

        public void Commit()
        {
            _context.SaveChanges();

            foreach (var entry in _context.ChangeTracker.Entries())
                _context.Entry(entry.Entity).State = EntityState.Detached;
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
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
