using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace denemesig
{
    /// <summary>
    /// Chat hub.
    /// This class is subclass of superclass SignalR Hub.
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>
        /// The cloud save.
        /// </summary>
        private ICloudStorage _cloudSave;

        /// <summary>
        /// The user count.
        /// </summary>
        /// <remarks>
        /// Online users' count in the system
        /// </remarks>
        private static int _userCount = 0;

        /// <summary>
        /// Dictionary that holds data of connected clients' connection ID and username
        /// </summary>
        /// <remarks>
        /// Online users' connection ID and username key value pairs in the system
        /// </remarks>
        private static Dictionary<string, string> _userConnName = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:denemesig.ChatHub"/> class.
        /// </summary>
        /// <remarks>
        /// Dependency Injection of ICloudStorage.
        /// </remarks>
        /// <param name="_save">Save.</param>
        public ChatHub(ICloudStorage _save)
        {
            this._cloudSave = _save;
        }

        /// <summary>
        /// Sends the message to all clients who are connected to <see cref="T:denemesig.ChatHub"/>
        /// </summary>
        /// <remarks>
        /// Send message to all connected clients asynchronously.
        /// Set username in _userConnName if it's first message of <paramref name="user"/>.
        /// </remarks>
        /// <returns>Async Task</returns>
        /// <param name="user">User.</param>
        /// <param name="message">Message.</param>
        //send message to all connected clients asynchronously
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

        /// <summary>
        /// This method triggers when the client successfully connects to the hub.
        /// Increase online user count.
        /// Send connection Id of new connected client to all online clients.
        /// <remarks>
        /// Triggers after new client connects to the <see cref="T:denemesig.ChatHub"/>
        /// </remarks>
        /// </summary>
        /// <returns>Task completed</returns>
        //works after client connects to ChatHub
        public override Task OnConnectedAsync()
        {
            _userCount++;
            _userConnName.Add(Context.ConnectionId, Context.ConnectionId);
            base.OnConnectedAsync();
            SendAllClients();
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method triggers when the client disconnects from the hub.
        /// Decrease online user count.
        /// Update connection Ids in frontend according to newly disconnected client.
        /// <remarks>
        /// Triggers after a client disconnects from the <see cref="T:denemesig.ChatHub"/>
        /// </remarks>
        /// </summary>
        /// <returns>Task completed</returns>
        /// <param name="exception">Exception.</param>
        //works after client disconnects from hub
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _userCount--;
            _userConnName.Remove(Context.ConnectionId);
            base.OnDisconnectedAsync(exception);
            SendAllClients();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends all clients.
        /// Updates online user count and online username list of all connected clients.
        /// </summary>
        private void SendAllClients()
        {
            Clients.All.SendAsync("UpdateCount", _userCount);
            Clients.All.SendAsync("UpdateUserList", _userConnName.Values.ToList());
        }
    }
}
