using Magazine.API.ClientCommunicationService.Contracts;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Infotecs.Magazine.API.ClientCommunicationService.Services
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
}
