using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BB.BL;
using BB.BL.Domain;
using BB.DAL;
using BB.DAL.EFUser;
using BB.UI.Web.MVC.Models;
using Microsoft.AspNet.SignalR;
using VideoLibrary;

namespace BB.UI.Web.MVC.Controllers.Utils
{
    public class PlaylistHub : Hub
    {
        private static readonly Dictionary<string, CurrentListenerModel> connectedGroupUsers = new Dictionary<string, CurrentListenerModel>();
        private static readonly Dictionary<string, string> playlistMasters = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> lastListener = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> lastJoiners = new Dictionary<string, string>(); 
        private static readonly UserManager userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddy)));

        public void AddTrack(string groupName)
        {
            Thread.Sleep(1000);
            Clients.OthersInGroup(groupName).addNewMessageToPage();
        }

        public void JoinGroup(string groupName)
        {
            if (lastJoiners.ContainsKey(Context.ConnectionId))
            {
                lastJoiners.Remove(Context.ConnectionId);
            }
            lastJoiners.Add(Context.ConnectionId, groupName);
            Groups.Add(Context.ConnectionId, groupName);
            var model = new CurrentListenerModel { GroupName = groupName };
            if (Context.User != null)
            {
                var user = userManager.ReadUser(Context.User.Identity.Name);
                model.User = user;
            }
            else
            {
                model.User = null;
            }
            if (connectedGroupUsers.ContainsKey(Context.ConnectionId))
            {
                connectedGroupUsers.Remove(Context.ConnectionId);
            }
            connectedGroupUsers.Add(Context.ConnectionId, model);

            Clients.Caller.modifyListeners(connectedGroupUsers.Values.ToList().FindAll(p => p.GroupName == model.GroupName).Count + " party people attending", connectedGroupUsers.Values.ToList().FindAll(p => p.GroupName == model.GroupName));
            Clients.OthersInGroup(groupName).modifyListeners(connectedGroupUsers.Values.ToList().FindAll(p => p.GroupName == model.GroupName).Count + " party people attending", connectedGroupUsers.Values.ToList().FindAll(p => p.GroupName == model.GroupName));
            if (playlistMasters.ContainsKey(groupName))
            {
                Clients.Client(playlistMasters.Single(p => p.Key == groupName).Value).syncLiveWhenPlaying();
            }
        }

        public void SyncLive(string groupName, CurrentPlayingViewModel track, float duration)
        {
            
            
            List<string> keys = new List<string>();
            foreach (var key in lastListener.Where(p=>p.Value == groupName).Select(p=>p.Key))
            {
                Clients.Client(key).playLive(track, (int) duration);
                keys.Add(key);
            }

            //Sending YouTube links after .playLive because it can take some time
            var youTube = YouTube.Default;
            var video = youTube.GetVideo("https://www.youtube.com/watch?v=" + track.TrackId);
            var youtubeLink = video.Uri;
            foreach (var key in keys)
            {
                Clients.Client(key).onPlaylinkGeneratedSync(youtubeLink, (int) duration);
                lastListener.Remove(key);
            }
        }

        public void SyncLiveWhenPlaying(string groupName, CurrentPlayingViewModel track, float duration)
        {
            var removeKeys = new List<string>();
            foreach (var key in lastJoiners.Where(p => p.Value == groupName).Select(p => p.Key))
            {
                Clients.Client(key).playLive(track, (int)duration);
                removeKeys.Add(key);
            }
            foreach (var removeKey in removeKeys)
            {
                lastJoiners.Remove(removeKey);
            }
        }

        public void PlayLive(string groupName)
        {
            if (playlistMasters.ContainsKey(groupName))
            {
                lastListener.Add(Context.ConnectionId, groupName);
                Clients.Client(playlistMasters.Single(p => p.Key == groupName).Value).syncLive();
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var key = connectedGroupUsers.Keys.Single(p => p.Equals(Context.ConnectionId));
            var model = connectedGroupUsers.FirstOrDefault(f => f.Key.Equals(Context.ConnectionId)).Value;
            if (playlistMasters.Values.Any(p => p.Equals(Context.ConnectionId)))
                {
                    Clients.OthersInGroup(model.GroupName).stopMusicPlaying();
                    playlistMasters.Remove(model.GroupName);
                }
            
            connectedGroupUsers.Remove(key);
            Clients.Group(model.GroupName).modifyListeners(connectedGroupUsers.Values.ToList().FindAll(p => p.GroupName == model.GroupName).Count + " party people attending", connectedGroupUsers.Values);
            return base.OnDisconnected(stopCalled);
        }

        public void StopPlaying(string groupName)
        {
            if (playlistMasters.Values.Any(p => p.Equals(Context.ConnectionId)))
            {
                Clients.OthersInGroup(groupName).stopMusicPlaying();
                playlistMasters.Remove(groupName);
            }
         
        }

        public void StartPlaying(CurrentPlayingViewModel track, string groupName)
        {
            if (playlistMasters.ContainsKey(groupName))
            {
                playlistMasters.Remove(groupName);
            }
            playlistMasters.Add(groupName, Context.ConnectionId);
            if (lastJoiners.ContainsKey(Context.ConnectionId))
            {
                lastJoiners.Remove(Context.ConnectionId);
            }
            Clients.OthersInGroup(groupName).playLive(track, 0);
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