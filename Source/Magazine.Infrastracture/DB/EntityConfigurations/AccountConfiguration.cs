using Infotecs.Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infotecs.Magazine.Infrastracture.DB.EntityConfigurations
{
    /// <summary>
    /// Конфигурация БД сущности "Пользователь" <see cref="Account"/>.
    /// </summary>
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account");

            builder.HasKey(e => e.Id).HasName("pk_account");

            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("INT").UseIdentityAlwaysColumn().IsRequired();
            builder.Property(e => e.Login).HasColumnName("login").HasColumnType("VARCHAR(320)").HasMaxLength(320).IsRequired();
            builder.Property(e => e.Password).HasColumnName("password").HasColumnType("CHAR(64)").HasMaxLength(64).IsFixedLength().IsRequired();
            builder.Property(e => e.Salt).HasColumnName("salt").HasColumnType("CHAR(24)").HasMaxLength(24).IsFixedLength().IsRequired();

            builder.HasIndex(e => e.Login).HasName("unq_account_login").IsUnique();
        }
    }
}
