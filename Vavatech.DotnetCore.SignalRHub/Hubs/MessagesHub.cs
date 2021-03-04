using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vavatech.DotnetCore.SignalRHub.Hubs
{
    // [Authorize]
    public class MessagesHub : Hub
    {
        Random random = new Random();

        public override Task OnConnectedAsync()
        {
            string groupName = $"Grupa{random.Next(1, 3)}";
            
            Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            // await Clients.All.SendAsync("YouHaveGotMessage", message);

            // await Clients.Others.SendAsync("YouHaveGotMessage", message);

            // All - Caller = Others

            await Clients.Group("Grupa1").SendAsync("YouHaveGotMessage", message);
        }

        public async Task Ping()
        {
            await Clients.Caller.SendAsync("Pong");
        }

    }
}
