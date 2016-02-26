using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BB.BL.Domain.Playlists
{
    public class Playlist
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public int MaximumVotesPerUser { get; set; }
        public bool Active { get; set; }
        public string ImageUrl { get; set; }
        public long? PlaylistMasterId { get; set; }
        public long? CreatedById { get; set; }
        public string Description { get; set; }
        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Comment> ChatComments { get; set; }
    }
}
