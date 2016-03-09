using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;


namespace BB.UI.Web.MVC.Controllers.Utils
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, string groupName)
        {
            Clients.Group(groupName).BroadcastMessage(name, message);
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(this.Context.ConnectionId, groupName);
        }
    }
}