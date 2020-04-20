using FluentNHibernate.Mapping;
using Magazine.Domain.Entities;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    /// <summary>
    /// Конфигурация БД сущности "Пользователь" <see cref="User"/>.
    /// </summary>
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("user");

            Id(x => x.Id).Column("id").CustomSqlType("INT").GeneratedBy.TriggerIdentity().Index("pk_user").Unique().Not.Nullable();

            Map(e => e.Login).Column("login").CustomSqlType("VARCHAR(320)").Length(320).Not.Nullable().Index("unq_user_login");
            Map(e => e.Password).Column("password").CustomSqlType("CHAR(64)").Length(64).Not.Nullable();
            Map(e => e.Salt).Column("salt").CustomSqlType("CHAR(24)").Length(24).Not.Nullable();
        }
    }
}
