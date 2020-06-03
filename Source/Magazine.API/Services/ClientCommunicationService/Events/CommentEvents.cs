using Magazine.API.Contracts.Event;

namespace Magazine.API.Services.ClientCommunicationService.Events
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
}
