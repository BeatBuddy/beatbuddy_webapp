using System;
using BB.BL.Domain.Users;

namespace BB.BL.Domain.Playlists
{
    public class Comment
    {
        public long CommentId { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public User User { get; set; }
        public Playlist Playlist { get; set; }
    }
}
