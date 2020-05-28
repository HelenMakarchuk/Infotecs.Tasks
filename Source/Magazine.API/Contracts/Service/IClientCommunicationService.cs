using System.Threading.Tasks;

namespace Magazine.API.Contracts.Service
{
    /// <summary>
    /// Сервис взаимодействия с клиентом.
    /// </summary>
    public interface IClientCommunicationService
    {
        /// <summary>
        /// Взаимодействие при обновлении статьи.
        /// </summary>
        Task СommunicateOnUpdate();
    }
}
