using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace denemesig
{
    public class ChatHub : Hub
    {
        // how many users online
        private static int _userCount = 0;

        //online users connection IDs
        private static List<string> _userList = new List<string>();

        //send message to all connected clients
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        //works after client connects to hub
        public override Task OnConnectedAsync()
        {
            _userCount++;
            _userList.Add(Context.ConnectionId);
            base.OnConnectedAsync();
            this.Clients.All.SendAsync("UpdateCount", _userCount);
            this.Clients.All.SendAsync("UpdateUserList", _userList);
            return Task.CompletedTask;
        }

        //works after client disconnects from hub
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _userCount--;
            _userList.Remove(Context.ConnectionId);
            base.OnDisconnectedAsync(exception);
            this.Clients.All.SendAsync("UpdateCount", _userCount);
            this.Clients.All.SendAsync("UpdateUserList", _userList);
            return Task.CompletedTask;
        }


    }
}
