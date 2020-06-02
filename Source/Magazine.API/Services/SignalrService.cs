using Infotecs.Magazine.Domain.Entities;
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
        Task Send(EntityServiceEvent serviceEvent);
    }

    /// <summary>
    /// Сервис взаимодействия с клиентом с использованием библиотеки SignalR.
    /// </summary>
    public class SignalrService : Hub<ISignalrClient>, IClientCommunicationService
    {
        public async Task Send(EntityServiceEvent serviceEvent)
        {
            await Clients.Others.Send(serviceEvent);
        }
    }

    /// <summary>
    /// Событие сервиса сущности.
    /// </summary>
    public class EntityServiceEvent
    {
        /// <summary>
        /// Название класса сервиса.
        /// </summary>
        public string ClassName { get; set; }
    }

    /// <summary>
    /// Событие сервиса сущности "Статья".
    /// </summary>
    public class ArticleServiceEvent : EntityServiceEvent { }

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
}
