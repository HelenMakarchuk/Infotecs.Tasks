using Newtonsoft.Json;

namespace Magazine.API.Contracts.Event
{
    /// <summary>
    /// Событие сервиса сущности "Статья".
    /// </summary>
    public abstract class ArticleServiceEvent : EntityServiceEvent
    {
        [JsonProperty]
        /// <summary>
        /// Идентификатор статьи.
        /// </summary>
        protected int Id { get; set; }
    }
}
