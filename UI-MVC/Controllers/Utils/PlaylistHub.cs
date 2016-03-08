using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BB.UI.Web.MVC.Controllers.Utils
{
    public class PlaylistHub : Hub
    {
        public void AddTrack()
        {
            Thread.Sleep(1000);
            Clients.All.addNewMessageToPage();
        }
    }
}