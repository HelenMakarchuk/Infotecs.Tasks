using System.Threading.Tasks;

namespace Magazine.API.ClientCommunicationService.Contracts
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
