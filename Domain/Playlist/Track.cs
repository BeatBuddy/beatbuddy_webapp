using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain
{
    public class Track
    {
        public long TrackId { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public TrackSource TrackSource { get; set; }
        public string CoverArtUrl { get; set; }
        public Collection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
