using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain
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
