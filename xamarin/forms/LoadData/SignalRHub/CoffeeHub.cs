using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRHub
{
    public class CoffeeHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceivedMessage", user, message);
        }
    }
}