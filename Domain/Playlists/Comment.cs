using System;
using BB.BL.Domain.Users;
using Newtonsoft.Json;

namespace BB.BL.Domain.Playlists
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public User User { get; set; }
        [JsonIgnore]
        public virtual Playlist Playlist { get; set; }
        [JsonIgnore]
        public long? PlaylistId { get; set; }
    }
}
