using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Users;

namespace BB.BL.Domain.Playlists
{
    public class PlaylistTrack
    {
        public long PlaylistTrackId { get; set; }
        public bool AlreadyPlayed { get; set; }
        public Track Track { get; set; }
        public Playlist Playlist { get; set; }
        public Collection<Vote> Votes { get; set; }
    }
}
