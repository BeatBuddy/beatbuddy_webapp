using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain.Playlists
{
    public class TrackSource
    {
        public long Id { get; set; }
        public SourceType SourceType { get; set; }
        public string Url { get; set; }
    }
}
