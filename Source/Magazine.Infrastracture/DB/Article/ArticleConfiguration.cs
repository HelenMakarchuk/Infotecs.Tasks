using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infotecs.Magazine.Infrastracture.DB.Article
{
    /// <summary>
    /// Конфигурация БД сущности "Статья" <see cref="Article"/>.
    /// </summary>
    public class ArticleConfiguration : IEntityTypeConfiguration<Domain.Article.Article>
    {
        public void Configure(EntityTypeBuilder<Domain.Article.Article> builder)
        {
            builder.ToTable("article");

            builder.HasKey(e => e.Id).HasName("pk_article");

            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Title).HasColumnName("title").HasColumnType("VARCHAR(80)").HasMaxLength(80).IsRequired();
            builder.Property(e => e.Teaser).HasColumnName("teaser").HasColumnType("BYTEA").IsRequired(false);
            builder.Property(e => e.Body).HasColumnName("body").HasColumnType("VARCHAR(60000)").HasMaxLength(60000).IsRequired();
            builder.Property(e => e.AccountId).HasColumnName("accountid").HasColumnType("INT").IsRequired();

            builder.HasOne(e => e.Account).WithMany().HasForeignKey(e => e.AccountId).HasConstraintName("fk_article_accountid_account_id").OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.Title).HasName("unq_article_title").IsUnique();
            builder.HasCheckConstraint("chk_article_body", "LENGTH(body) >= 2000");
        }
    }
}
