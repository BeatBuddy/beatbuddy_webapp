using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain
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
