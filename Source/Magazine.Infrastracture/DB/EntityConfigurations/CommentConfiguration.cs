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

            builder.HasKey(e => e.Id).HasName("pk_Comment");

            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Body).HasColumnName("body").HasColumnType("VARCHAR(6000)").HasMaxLength(6000).IsRequired();
            builder.Property(e => e.ArticleId).HasColumnName("articleId").HasColumnType("INT").IsRequired();
            builder.Property(e => e.UserId).HasColumnName("userId").HasColumnType("INT").IsRequired();

            builder.HasOne(e => e.Article).WithMany(e => e.Comments).HasForeignKey(e => e.ArticleId).HasConstraintName("fk_comment_articleId_article_Id").OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.User).WithMany(e => e.Comments).HasForeignKey(e => e.UserId).HasConstraintName("fk_Comment_userId_user_Id").OnDelete(DeleteBehavior.NoAction);

            builder.HasCheckConstraint("chk_comment_body", "LENGTH(TRIM(Body)) > 0");
        }
    }
}