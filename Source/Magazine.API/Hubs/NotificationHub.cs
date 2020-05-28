using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Infotecs.Magazine.API.Hubs
{
    public interface INotificationHub
    {
        Task NotifyOnUpdate();
    }

    public interface INotificationClient
    {
        Task NotifyOnUpdate(string message);
    }

    public class NotificationHub : Hub<INotificationClient>, INotificationHub
    {
        public async Task NotifyOnUpdate()
        {
            await Clients.Others.NotifyOnUpdate("This article was changed by another user. Refresh this article to get last changes.");
        }
    }
}
