using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int Score { get; set; }
        public User User { get; set; }
        public PlaylistTrack PlaylistTrack { get; set; }
    }
}
