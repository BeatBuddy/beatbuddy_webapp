using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.DAL;
using BB.DAL.EFUser;
using BB.UI.Web.MVC.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using VideoLibrary;

namespace BB.UI.Web.MVC.Controllers.Utils
{
    public class PlaylistHub : Hub
    {
        private static Dictionary<string, string> connectedGroupUsers = new Dictionary<string, string>(); 
        private static List<User> users = new List<User>(); 
        private static UserManager userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddy)));
        public void AddTrack(string groupName)
        {
            Thread.Sleep(1000);
            Clients.OthersInGroup(groupName).addNewMessageToPage();
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
            if (Context.User != null)
            {
                var user = userManager.ReadUser(Context.User.Identity.Name);
                if (!users.Contains(user))
                {
                    users.Add(user);
                }
            }
            else
            {
                users.Add(null);
            }
            if (connectedGroupUsers.ContainsKey(Context.ConnectionId))
            {
                connectedGroupUsers.Remove(Context.ConnectionId);
            }
                connectedGroupUsers.Add(Context.ConnectionId, groupName);

            Clients.Caller.modifyListeners(connectedGroupUsers.Values.ToList().FindAll(p => p.Equals(groupName)).Count + " party people attending", users);
            Clients.OthersInGroup(groupName).modifyListeners(connectedGroupUsers.Values.ToList().FindAll(p => p.Equals(groupName)).Count + " party people attending", users);
        }

        public override Task OnConnected()
        {
            Clients.All.joinGroup();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User != null)
            {
                users.Remove(userManager.ReadUser(Context.User.Identity.Name));
            }
            else
            {
                users.Remove(users.First(u => u == null));
            }
            var groupName = connectedGroupUsers.FirstOrDefault(f => f.Key.Equals(Context.ConnectionId)).Value;
            var key = connectedGroupUsers.Keys.Single(p => p.Equals(Context.ConnectionId));
            connectedGroupUsers.Remove(key);
            Clients.Group(groupName).modifyListeners(connectedGroupUsers.Values.ToList().FindAll(p => p.Equals(groupName)).Count + " party people attending", users);
            return base.OnDisconnected(stopCalled);
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