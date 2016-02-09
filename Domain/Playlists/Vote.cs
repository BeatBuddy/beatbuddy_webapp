using BB.BL.Domain.Users;

namespace BB.BL.Domain.Playlists
{
    public class Vote
    {
        public long Id { get; set; }
        public int Score { get; set; }
        public User User { get; set; }
    }
}
