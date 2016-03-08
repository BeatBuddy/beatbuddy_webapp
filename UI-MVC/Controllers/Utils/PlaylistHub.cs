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
        public void AddTrack()
        {
            Thread.Sleep(1000);
            Clients.All.addNewMessageToPage();
        }

        public void StartPlaying(CurrentPlayingViewModel track)
        {
            Clients.All.startMusicPlaying(track);
        }
    }
}