using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Magazine.Web.Hubs
{
    public class MessageHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("broadcastMessage", message);
        }
    }
}
