using FluentNHibernate.Mapping;
using Magazine.Domain.Entities;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    /// <summary>
    /// Конфигурация БД сущности "Пользователь" <see cref="Account"/>.
    /// </summary>
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Table("account");

            Id(x => x.Id, "id").CustomSqlType("INT").GeneratedBy.TriggerIdentity().Index("pk_account").Unique().Not.Nullable();

            Map(e => e.Login, "login").CustomSqlType("VARCHAR(320)").Length(320).Not.Nullable().Index("unq_account_login");
            Map(e => e.Password, "password").CustomSqlType("CHAR(64)").Length(64).Not.Nullable();
            Map(e => e.Salt, "salt").CustomSqlType("CHAR(24)").Length(24).Not.Nullable();
        }
    }
}
