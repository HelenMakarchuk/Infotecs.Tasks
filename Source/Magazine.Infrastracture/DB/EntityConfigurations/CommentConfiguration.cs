using Infotecs.Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infotecs.Magazine.Infrastracture.DB.EntityConfigurations
{
    /// <summary>
    /// Конфигурация БД сущности "Комментарий" <see cref="Comment"/>.
    /// </summary>
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comment");

            builder.HasKey(e => e.Id).HasName("pk_comment");

            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Body).HasColumnName("body").HasColumnType("VARCHAR(6000)").HasMaxLength(6000).IsRequired();
            builder.Property(e => e.ArticleId).HasColumnName("articleid").HasColumnType("INT").IsRequired();
            builder.Property(e => e.AccountId).HasColumnName("accountid").HasColumnType("INT").IsRequired();

            builder.HasCheckConstraint("chk_comment_body", "LENGTH(TRIM(body)) > 0");
        }
    }
}