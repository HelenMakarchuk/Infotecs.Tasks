using FluentNHibernate.Mapping;
using Magazine.Domain.Entities;

namespace Magazine.Infrastracture.DB.EntityConfigurations
{
    /// <summary>
    /// Конфигурация БД сущности "Комментарий" <see cref="Comment"/>.
    /// </summary>
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("comment");

            Id(x => x.Id, "id").CustomSqlType("INT").GeneratedBy.TriggerIdentity().Index("pk_comment").Unique().Not.Nullable();

            Map(e => e.Body, "body").CustomSqlType("VARCHAR(6000)").Length(6000).Not.Nullable().Check("LENGTH(TRIM(body)) > 0");
            Map(e => e.ArticleId, "articleid").CustomSqlType("INT").Not.Nullable();
            Map(e => e.UserId, "userid").CustomSqlType("INT").Not.Nullable();

            HasOne(e => e.Article).ForeignKey("fk_comment_articleid_article_id").Cascade.None();
            HasOne(e => e.User).ForeignKey("fk_comment_userid_user_id").Cascade.None();
        }
    }
}