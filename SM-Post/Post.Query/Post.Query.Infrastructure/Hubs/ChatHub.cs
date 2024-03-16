using CQRS.Core.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Post.Query.Infrastructure.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendMessage(message);
        }
    }
}