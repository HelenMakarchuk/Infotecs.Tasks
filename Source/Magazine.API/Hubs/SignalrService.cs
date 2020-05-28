using Magazine.API.Contracts.Service;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Infotecs.Magazine.API.Hubs
{
    /// <summary>
    /// Интерфейс клиента библиотеки SignalR.
    /// </summary>
    public interface ISignalrClient
    {
        Task СommunicateOnUpdate(string message);
    }

    /// <summary>
    /// Сервис взаимодействия с клиентом с использованием библиотеки SignalR.
    /// </summary>
    public class SignalrService : Hub<ISignalrClient>, IClientCommunicationService
    {
        public async Task СommunicateOnUpdate()
        {
            await Clients.Others.СommunicateOnUpdate("This article was changed by another user. Refresh this article to get last changes.");
        }
    }
}
