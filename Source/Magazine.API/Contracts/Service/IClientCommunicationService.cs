using Infotecs.Magazine.API.Services;
using System.Threading.Tasks;

namespace Magazine.API.Contracts.Service
{
    /// <summary>
    /// Сервис взаимодействия с клиентом.
    /// </summary>
    public interface IClientCommunicationService
    {
        /// <summary>
        /// Вызов события клиента.
        /// </summary>
        Task Send(EntityServiceEvent serviceEvent);
    }
}
