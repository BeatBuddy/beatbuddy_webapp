using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BB.BL.Domain.Playlists
{
    public class PlaylistTrack
    {
        public long Id { get; set; }
        public DateTime? PlayedAt { get; set; }

        [JsonIgnore]
        public virtual ICollection<Vote> Votes { get; set; }

        public Track Track { get; set; }

        [JsonIgnore]
        public virtual Playlist Playlist { get; set; }

        [JsonIgnore]
        public long? PlaylistId { get; set; }
    }
}
