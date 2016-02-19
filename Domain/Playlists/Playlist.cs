using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BB.BL.Domain.Playlists
{
    public class Playlist
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public int MaximumVotesPerUser { get; set; }
        public bool Active { get; set; }
        public string ImageUrl { get; set; }
        public long? PlaylistMasterId { get; set; }
        public long? CreatedById { get; set; }
        public Collection<PlaylistTrack> PlaylistTracks { get; set; }
        public Collection<Comment> Comments { get; set; }
        public Collection<Comment> ChatComments { get; set; }
    }
}
