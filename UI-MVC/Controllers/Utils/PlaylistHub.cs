using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using BB.BL.Domain.Playlists;
using BB.UI.Web.MVC.Models;
using Microsoft.AspNet.SignalR;

namespace BB.UI.Web.MVC.Controllers.Utils
{
    public class PlaylistHub : Hub
    {
        public void AddTrack(string groupName)
        {
            Thread.Sleep(1000);
            Clients.Group(groupName).addNewMessageToPage();
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(this.Context.ConnectionId, groupName);
        }

        public void StartPlaying(CurrentPlayingViewModel track, string groupName)
        {
            Clients.Group(groupName).startMusicPlaying(track);
        }
    }
}