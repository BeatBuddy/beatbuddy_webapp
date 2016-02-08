using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain
{
    public class TrackSource
    {
        public long TrackSourceId { get; set; }
        public SourceType SourceType { get; set; }
        public string Url { get; set; }
        public Track Track { get; set; }
    }
}
