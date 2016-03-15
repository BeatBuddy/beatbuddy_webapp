using Microsoft.AspNet.SignalR;

namespace BB.UI.Web.MVC.Controllers.Utils
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, string image, string groupName)
        {
            Clients.Group(groupName).BroadcastMessage(name, message, image);
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(this.Context.ConnectionId, groupName);
        }
    }
}