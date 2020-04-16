using Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comment");

            builder.HasKey(e => e.Id).HasName("pk_comment");

            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Body).HasColumnName("body").HasColumnType("VARCHAR(6000)").HasMaxLength(6000).IsRequired();
            builder.Property(e => e.ArticleId).HasColumnName("articleid").HasColumnType("INT").IsRequired();
            builder.Property(e => e.UserId).HasColumnName("userid").HasColumnType("INT").IsRequired();

            builder.HasOne(e => e.Article).WithMany(e => e.Comments).HasForeignKey(e => e.ArticleId).HasConstraintName("fk_comment_articleid_article_id").OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.User).WithMany(e => e.Comments).HasForeignKey(e => e.UserId).HasConstraintName("fk_comment_userid_user_id").OnDelete(DeleteBehavior.NoAction);

            builder.HasCheckConstraint("chk_comment_body", "LENGTH(TRIM(body)) > 0");
        }
    }
}