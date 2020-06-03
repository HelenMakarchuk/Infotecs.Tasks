using Newtonsoft.Json;

namespace Magazine.API.Contracts.Event
{
    /// <summary>
    /// Событие сервиса сущности "Комментарий".
    /// </summary>
    public abstract class CommentServiceEvent : EntityServiceEvent
    {
        [JsonProperty]
        /// <summary>
        /// Идентификатор комментария.
        /// </summary>
        protected int Id { get; set; }
    }
}
