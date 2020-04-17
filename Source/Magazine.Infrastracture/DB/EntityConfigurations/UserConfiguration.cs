using Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    /// <summary>
    /// Конфигурация БД сущности "Пользователь" <see cref="User"/>.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(e => e.Id).HasName("pk_user");

            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Login).HasColumnName("login").HasColumnType("VARCHAR(320)").HasMaxLength(320).IsRequired();
            builder.Property(e => e.Password).HasColumnName("password").HasColumnType("CHAR(64)").HasMaxLength(64).IsFixedLength().IsRequired();
            builder.Property(e => e.Salt).HasColumnName("salt").HasColumnType("CHAR(24)").HasMaxLength(24).IsFixedLength().IsRequired();

            builder.HasIndex(e => e.Login).HasName("unq_user_login").IsUnique();
        }
    }
}
