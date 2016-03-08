using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using BB.BL.Domain.Playlists;
using BB.UI.Web.MVC.Models;
using Microsoft.AspNet.SignalR;
using VideoLibrary;

namespace BB.UI.Web.MVC.Controllers.Utils
{
    public class PlaylistHub : Hub
    {
        public void AddTrack(string groupName)
        {
            Thread.Sleep(1000);
            Clients.OthersInGroup(groupName).addNewMessageToPage();
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(this.Context.ConnectionId, groupName);
        }

        public void StartPlaying(CurrentPlayingViewModel track, string groupName)
        {
            Clients.OthersInGroup(groupName).startMusicPlaying(track);
            var youTube = YouTube.Default; // starting point for YouTube actions
            var video = youTube.GetVideo("https://www.youtube.com/watch?v=" + track.TrackId); // gets a Video object with info about the video
            Clients.OthersInGroup(groupName).onPlaylinkGenerated(video.Uri);
        }
        public void PausePlaying(string groupName)
        {
            Clients.OthersInGroup(groupName).pauseMusicPlaying();
        }
        public void ResumePlaying(float duration, string groupName)
        {
            Clients.OthersInGroup(groupName).resumeMusicPlaying(duration);
        }
    }
}