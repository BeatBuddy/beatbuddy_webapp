using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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
