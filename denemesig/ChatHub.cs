using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace denemesig
{
    public class ChatHub : Hub
    {
        // how many users online
        private static int _userCount = 0;

        //online users connection IDs and usernames key value pairs
        private static Dictionary<string, string> _userConnName = new Dictionary<string, string>();

        //send message to all connected clients
        public async Task SendMessage(string user, string message)
        {
            if (_userConnName.ContainsKey(Context.ConnectionId))
            {
                _userConnName[Context.ConnectionId] = user;
                await this.Clients.All.SendAsync("UpdateUserList", _userConnName.Values.ToList());
            }

            await Clients.All.SendAsync("ReceiveMessage", user, message);

            await InsertMessageToDB(Context.ConnectionId, user, message);

            //await Clients.All.SendAsync("UpdateRetrievedData", RetrieveMessageFromDB(Context.ConnectionId, user).ToString());
        }

        //works after client connects to hub
        public override Task OnConnectedAsync()
        {
            _userCount++;
            _userConnName.Add(Context.ConnectionId, Context.ConnectionId);
            base.OnConnectedAsync();
            this.Clients.All.SendAsync("UpdateCount", _userCount);
            this.Clients.All.SendAsync("UpdateUserList", _userConnName.Values.ToList());
            return Task.CompletedTask;
        }

        //works after client disconnects from hub
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _userCount--;
            _userConnName.Remove(Context.ConnectionId);
            base.OnDisconnectedAsync(exception);
            this.Clients.All.SendAsync("UpdateCount", _userCount);
            this.Clients.All.SendAsync("UpdateUserList", _userConnName.Values.ToList());
            return Task.CompletedTask;
        }

        //TODO: move this method to controller class
        private async Task InsertMessageToDB(string conID, string user, string msg)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=denemesigstorage;AccountKey=9Rr4AuGxYzS2veA5eYxblu/gobva4A048xbrk/2g03FHnewEY70vB1xfaj0QvSTRY8zE3ezc7uFJ7a4/aAgGvQ==;EndpointSuffix=core.windows.net");
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("denemesigmessage");

            await table.CreateIfNotExistsAsync();
            MessageEntity messageEntity = new MessageEntity(conID, user, msg, DateTime.Now);
            TableOperation insertOp = TableOperation.Insert(messageEntity);
            await table.ExecuteAsync(insertOp);

            //return client.GetTableReference("connection");
        }

    }
}
