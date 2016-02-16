namespace BB.BL.Domain.Playlists
{
    public class TrackSource
    {
        public long Id { get; set; }
        public SourceType SourceType { get; set; }
        public string Url { get; set; }
        public string TrackId { get; set; }
    }
}
