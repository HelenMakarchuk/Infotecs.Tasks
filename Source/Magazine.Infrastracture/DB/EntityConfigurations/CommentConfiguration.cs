using Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment");

            builder.HasKey(e => e.Id).HasName("PK_Comment");

            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Body).HasColumnName("Body").HasColumnType("VARCHAR(6000)").HasMaxLength(6000).IsRequired();
            builder.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("INT").IsRequired();
            builder.Property(e => e.UserId).HasColumnName("UserId").HasColumnType("INT").IsRequired();

            builder.HasOne(e => e.Article).WithMany(e => e.Comments).HasForeignKey(e => e.ArticleId).HasConstraintName("FK_Comment_ArticleId_Article_Id").OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.User).WithMany(e => e.Comments).HasForeignKey(e => e.UserId).HasConstraintName("FK_Comment_UserId_User_Id").OnDelete(DeleteBehavior.NoAction);

            builder.HasCheckConstraint("CHK_Comment_Body", "LENGTH(TRIM(Body)) > 0");
        }
    }
}