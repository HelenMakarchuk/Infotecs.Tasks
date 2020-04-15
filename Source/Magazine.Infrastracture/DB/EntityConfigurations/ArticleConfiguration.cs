using Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Article");

            builder.HasKey(e => e.Id).HasName("PK_Article");

            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Title).HasColumnName("Title").HasColumnType("VARCHAR(80)").HasMaxLength(80).IsRequired();
            builder.Property(e => e.Teaser).HasColumnName("Teaser").HasColumnType("BYTEA").IsRequired(false);
            builder.Property(e => e.Body).HasColumnName("Body").HasColumnType("VARCHAR(60000)").HasMaxLength(60000).IsRequired();
            builder.Property(e => e.UserId).HasColumnName("UserId").HasColumnType("INT").IsRequired();

            builder.HasOne(e => e.User).WithMany(e => e.Articles).HasForeignKey(e => e.UserId).HasConstraintName("FK_Article_UserId_User_Id").OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.Title).HasName("UNQ_Article_Title").IsUnique();
            builder.HasCheckConstraint("CHK_Article_Body", "LENGTH(Body) >= 2000");
        }
    }
}
