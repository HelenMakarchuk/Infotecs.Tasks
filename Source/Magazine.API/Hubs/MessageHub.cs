using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Magazine.API.Hubs
{
    public class MessageHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("broadcastMessage", message);
        }
    }
}
