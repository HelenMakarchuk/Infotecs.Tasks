using Magazine.Domain.Entities;
using Magazine.Infrastracture.DB.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Magazine.Infrastracture.DB
{
    public class Context : DbContext
    {
        public Context()
        {
            Database.EnsureCreated();
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(ConfigurationManager.ConnectionStrings["InfotecsMagazine"]?.ConnectionString);
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
        }
    }
}