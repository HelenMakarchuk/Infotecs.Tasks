using Magazine.API.ClientCommunicationService.Events;
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
        /// Аргумент события.
        /// </summary>
        protected CommentEventArgument EventArgument { get; set; }
    }
}
