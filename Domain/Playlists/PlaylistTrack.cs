using System.Collections.Generic;

namespace BB.BL.Domain.Playlists
{
    public class PlaylistTrack
    {
        public long Id { get; set; }
        public bool AlreadyPlayed { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public Track Track { get; set; }
    }
}
