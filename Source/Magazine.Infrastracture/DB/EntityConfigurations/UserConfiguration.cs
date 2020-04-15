using Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(e => e.Id).HasName("PK_User");

            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Login).HasColumnName("Login").HasColumnType("VARCHAR(320)").HasMaxLength(320).IsRequired();
            builder.Property(e => e.Password).HasColumnName("Password").HasColumnType("CHAR(64)").HasMaxLength(64).IsFixedLength().IsRequired();
            builder.Property(e => e.Salt).HasColumnName("Salt").HasColumnType("CHAR(24)").HasMaxLength(24).IsFixedLength().IsRequired();

            builder.HasIndex(e => e.Login).HasName("UNQ_User_Login").IsUnique();
        }
    }
}
