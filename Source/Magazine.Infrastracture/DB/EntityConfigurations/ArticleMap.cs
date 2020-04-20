using FluentNHibernate.Mapping;
using Magazine.Domain.Entities;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    /// <summary>
    /// Конфигурация БД сущности "Статья" <see cref="Article"/>.
    /// </summary>
    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("article");

            Id(x => x.Id).Column("id").CustomSqlType("INT").GeneratedBy.TriggerIdentity().Index("pk_article").Unique().Not.Nullable();

            Map(e => e.Title, "title").CustomSqlType("VARCHAR(80)").Length(80).Not.Nullable().Index("unq_article_title").Unique();
            Map(e => e.Teaser, "teaser").CustomSqlType("BYTEA").Nullable();
            Map(e => e.Body, "body").CustomSqlType("VARCHAR(60000)").Length(60000).Not.Nullable().Check("LENGTH(body) >= 2000");
            Map(e => e.UserId, "userid").CustomSqlType("INT").Not.Nullable();

            HasOne(e => e.User).ForeignKey("fk_article_userid_user_id").Cascade.None();
        }
    }
}
