using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain.Playlists
{
    public class Track
    {
        public long Id { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public TrackSource TrackSource { get; set; }
        public string CoverArtUrl { get; set; }
        
    }
}
