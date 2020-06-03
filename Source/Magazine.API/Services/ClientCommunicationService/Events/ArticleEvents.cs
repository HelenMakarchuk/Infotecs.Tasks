using Magazine.API.Contracts.Event;

namespace Magazine.API.Services.ClientCommunicationService.Events
{
    /// <summary>
    /// Событие "Добавление" сервиса сущности "Статья".
    /// </summary>
    public class ArticleServiceAddEvent : ArticleServiceEvent { }

    /// <summary>
    /// Событие "Обновление" сервиса сущности "Статья".
    /// </summary>
    public class ArticleServiceUpdateEvent : ArticleServiceEvent { }

    /// <summary>
    /// Событие "Удаление" сервиса сущности "Статья".
    /// </summary>
    public class ArticleServiceDeleteEvent : ArticleServiceEvent { }
}
