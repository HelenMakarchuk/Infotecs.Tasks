using Magazine.API.ClientCommunicationService.Contracts;
using Newtonsoft.Json;

namespace Magazine.API.ClientCommunicationService.Events
{
    /// <summary>
    /// Событие "Добавление" сервиса сущности "Комментарий".
    /// </summary>
    public class CommentServiceAddEvent : CommentServiceEvent { }

    /// <summary>
    /// Событие "Обновление" сервиса сущности "Комментарий".
    /// </summary>
    public class CommentServiceUpdateEvent : CommentServiceEvent { }

    /// <summary>
    /// Событие "Удаление" сервиса сущности "Комментарий".
    /// </summary>
    public class CommentServiceDeleteEvent : CommentServiceEvent { }

    /// <summary>
    /// Аргумент события комментария.
    /// </summary>
    public class CommentEventArgument
    {
        [JsonProperty]
        /// <summary>
        /// Идентификатор комментария.
        /// </summary>
        protected int Id { get; set; }

        [JsonProperty]
        /// <summary>
        /// Идентификатор статьи.
        /// </summary>
        protected int ArticleId { get; set; }
    }
}
