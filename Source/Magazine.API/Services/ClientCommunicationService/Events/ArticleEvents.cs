using Infotecs.Magazine.Domain.Entities;
using Magazine.API.Contracts.Event;

namespace Magazine.API.Services.ClientCommunicationService.Events
{
    /// <summary>
    /// Событие "Добавление" сервиса сущности "Статья".
    /// </summary>
    public class ArticleServiceAddEvent : ArticleServiceEvent
    {
        /// <summary>
        /// Статья.
        /// </summary>
        public Article Article { get; set; }
    }

    /// <summary>
    /// Событие "Обновление" сервиса сущности "Статья".
    /// </summary>
    public class ArticleServiceUpdateEvent : ArticleServiceEvent
    {
        /// <summary>
        /// Статья.
        /// </summary>
        public Article Article { get; set; }
    }

    /// <summary>
    /// Событие "Удаление" сервиса сущности "Статья".
    /// </summary>
    public class ArticleServiceDeleteEvent : ArticleServiceEvent
    {
        /// <summary>
        /// Идентификатор статьи.
        /// </summary>
        public int id { get; set; }
    }
}
