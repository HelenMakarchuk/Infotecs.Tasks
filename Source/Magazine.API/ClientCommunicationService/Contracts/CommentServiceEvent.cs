using Newtonsoft.Json;

namespace Magazine.API.ClientCommunicationService.Contracts
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
