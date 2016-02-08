using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;

namespace BB.BL.Domain.Playlists
{
    public class Playlist
    {
        public long PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public int MaxVotesPerUser { get; set; }
        public bool Active { get; set; }
        public string ImageUrl { get; set; }
        public Organisation Organisation { get; set; }
        public Collection<PlaylistTrack> PlaylistTracks { get; set; }
        public Collection<Comment> Comments { get; set; }
        public Collection<Comment> ChatComments { get; set; }
    }
}
