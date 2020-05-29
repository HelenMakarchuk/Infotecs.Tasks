using Magazine.API.Contracts.Service;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Infotecs.Magazine.API.Services
{
    /// <summary>
    /// Интерфейс клиента библиотеки SignalR.
    /// </summary>
    public interface ISignalrClient
    {
        Task СommunicateOnUpdate(ClientComponentEvent componentEvent);
    }

    /// <summary>
    /// Сервис взаимодействия с клиентом с использованием библиотеки SignalR.
    /// </summary>
    public class SignalrService : Hub<ISignalrClient>, IClientCommunicationService
    {
        public async Task СommunicateOnUpdate()
        {
            var componentEvent = new ArticleComponentEvent();
            componentEvent.ClassName = componentEvent.GetType().Name;
            componentEvent.Message = "This article was changed by another user. Refresh this article to get last changes.";

            await Clients.Others.СommunicateOnUpdate(componentEvent);
        }
    }

    /// <summary>
    /// Событие компонента приложения клиента.
    /// </summary>
    public abstract class ClientComponentEvent
    {
        /// <summary>
        /// Тип события.
        /// </summary>
        public string ClassName { get; set; }
    }

    /// <summary>
    /// Событие компонента "Статья".
    /// </summary>
    public class ArticleComponentEvent : ClientComponentEvent
    {
        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; set; }
    }
}
