using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebSocketServer.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the AddMessage method on all of the clients
            Clients.All.AddMessage(name, message);
        }
    }
}