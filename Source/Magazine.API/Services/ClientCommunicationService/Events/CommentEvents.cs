using Infotecs.Magazine.Domain.Entities;
using Magazine.API.Contracts.Event;

namespace Magazine.API.Services.ClientCommunicationService.Events
{
    /// <summary>
    /// Событие "Добавление" сервиса сущности "Комментарий".
    /// </summary>
    public class CommentServiceAddEvent : CommentServiceEvent
    {
        /// <summary>
        /// Комментарий.
        /// </summary>
        public Comment Comment { get; set; }
    }

    /// <summary>
    /// Событие "Обновление" сервиса сущности "Комментарий".
    /// </summary>
    public class CommentServiceUpdateEvent : CommentServiceEvent
    {
        /// <summary>
        /// Комментарий.
        /// </summary>
        public Comment Comment { get; set; }
    }

    /// <summary>
    /// Событие "Удаление" сервиса сущности "Комментарий".
    /// </summary>
    public class CommentServiceDeleteEvent : CommentServiceEvent
    {
        /// <summary>
        /// Идентификатор комментария.
        /// </summary>
        public int id { get; set; }
    }
}
