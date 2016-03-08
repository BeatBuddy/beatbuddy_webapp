
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BB.BL.Domain.Playlists;

namespace BB.UI.Web.MVC.Models
{
    /// <summary>
    /// ViewModel for Android that sends only the tracks which have not been played and personalized votes
    /// </summary>
    public class LivePlaylistViewModel
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public int MaximumVotesPerUser { get; set; }
        public bool Active { get; set; }
        public string ImageUrl { get; set; }
        public long? CreatedById { get; set; }
        public string Description { get; set; }
        public long? PlaylistMasterId { get; set; }
        public virtual ICollection<LivePlaylistTrackViewModel> PlaylistTracks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Comment> ChatComments { get; set; }
    }

    public class LivePlaylistTrackViewModel
    {
        public Track Track { get; set; }
        public int Score { get; set; }
        public int PersonalScore { get; set; }
    }
}