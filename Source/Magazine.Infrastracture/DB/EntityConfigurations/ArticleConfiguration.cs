using Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    /// <summary>
    /// Конфигурация БД сущности "Статья" <see cref="Article"/>.
    /// </summary>
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("article");

            builder.HasKey(e => e.Id).HasName("pk_article");

            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Title).HasColumnName("title").HasColumnType("VARCHAR(80)").HasMaxLength(80).IsRequired();
            builder.Property(e => e.Teaser).HasColumnName("teaser").HasColumnType("BYTEA").IsRequired(false);
            builder.Property(e => e.Body).HasColumnName("body").HasColumnType("VARCHAR(60000)").HasMaxLength(60000).IsRequired();
            builder.Property(e => e.UserId).HasColumnName("userid").HasColumnType("INT").IsRequired();

            builder.HasOne(e => e.User).WithMany(e => e.Articles).HasForeignKey(e => e.UserId).HasConstraintName("fk_article_userid_user_id").OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.Title).HasName("unq_article_title").IsUnique();
            builder.HasCheckConstraint("chk_article_body", "LENGTH(body) >= 2000");
        }
    }
}
