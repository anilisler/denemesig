using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace denemesig
{
    public class ChatHub : Hub
    {
        private ICloudStorage _cloudSave;

        // how many users online
        private static int _userCount = 0;

        //online users connection IDs and usernames key value pairs
        private static Dictionary<string, string> _userConnName = new Dictionary<string, string>();

        public ChatHub(ICloudStorage _save)
        {
            this._cloudSave = _save;
        }

        //send message to all connected clients
        public async Task SendMessage(string user, string message)
        {
            if (_userConnName.ContainsKey(Context.ConnectionId))
            {
                _userConnName[Context.ConnectionId] = user;
                await Clients.All.SendAsync("UpdateUserList", _userConnName.Values.ToList());
            }

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        
            _cloudSave.InsertMessage(Context.ConnectionId, user, message);
        }

        //works after client connects to hub
        public override Task OnConnectedAsync()
        {
            _userCount++;
            _userConnName.Add(Context.ConnectionId, Context.ConnectionId);
            base.OnConnectedAsync();
            SendAllClients();
            return Task.CompletedTask;
        }

        //works after client disconnects from hub
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _userCount--;
            _userConnName.Remove(Context.ConnectionId);
            base.OnDisconnectedAsync(exception);
            SendAllClients();
            return Task.CompletedTask;
        }

        private void SendAllClients()
        {
            Clients.All.SendAsync("UpdateCount", _userCount);
            Clients.All.SendAsync("UpdateUserList", _userConnName.Values.ToList());
        }
    }
}
