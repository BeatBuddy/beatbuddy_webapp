using System;
using BB.BL.Domain.Users;

namespace BB.BL.Domain.Playlists
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public User User { get; set; }
       
    }
}
