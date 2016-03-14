using System;
using System.Collections.Generic;

namespace BB.BL.Domain.Playlists
{
    public class PlaylistTrack
    {
        public long Id { get; set; }
        public DateTime? PlayedAt { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public Track Track { get; set; }
        public virtual Playlist Playlist { get; set; }
        public long? PlaylistId { get; set; }
    }
}
