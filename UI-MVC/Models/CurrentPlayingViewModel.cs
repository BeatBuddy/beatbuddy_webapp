using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BB.UI.Web.MVC.Models
{
    public class CurrentPlayingViewModel
    {
        public string TrackId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int NextTracks { get; set; }
        public string  CoverArtUrl { get; set; }
                
    }
}